
using System.Collections.Concurrent;
using System.Text;
using EventStore.Client;
using Microsoft.Extensions.Logging;
using Neo.Infrastructure.EventStore.Serializations;
using Neo.Infrastructure.Framework.Subscriptions;
using Neo.Infrastructure.Framework.Subscriptions.Consumers;
using Neo.Infrastructure.Framework.Subscriptions.Contexts;

namespace Neo.Infrastructure.EventStore.Subscriptions;

public abstract class PersistentSubscriptionBase<T> : EventSubscription<T>
    where T : PersistentSubscriptionOptions
{
    protected EventStorePersistentSubscriptionsClient SubscriptionClient { get; }
    protected PersistentSubscription Subscription;
    protected IEventSerializer EventSerializer { get; }
    protected DomainEventTypeMapper Mapper;

    const string ResolvedEventKey = "resolvedEvent";
    const string SubscriptionKey = "subscription";

    protected PersistentSubscriptionBase(
        EventStorePersistentSubscriptionsClient subscriptionClient,
        T options, 
        DomainEventTypeMapper mapper,
        IMessageConsumer messageConsumer,
        IEventSerializer eventSerializer,
        ILoggerFactory loggerFactory)
        : base(options, messageConsumer, loggerFactory)
    {
        SubscriptionClient = subscriptionClient;
        Mapper = mapper;
        EventSerializer = eventSerializer;
    }

    protected override async Task Subscribe(CancellationToken cancellationToken)
    {
        var settings = Options.SubscriptionSettings ??
                       new PersistentSubscriptionSettings(Options.ResolveLinkTos);
        try
        {
            Subscription = await LocalSubscribe(HandleEvent, HandleDrop, cancellationToken)
                .ConfigureAwait(false);
        }
        catch (PersistentSubscriptionNotFoundException)
        {
            await CreatePersistentSubscription(settings, cancellationToken);
            Subscription = await LocalSubscribe(HandleEvent, HandleDrop, cancellationToken)
                .ConfigureAwait(false);
        }
    }

    protected override async Task Unsubscribe(CancellationToken cancellationToken)
    {
        try
        {
            Stopping.Cancel(false);
            await Task.Delay(100, cancellationToken);
            Subscription?.Dispose();
        }
        catch (Exception)
        {
        }
    }

    private async Task HandleEvent(
        PersistentSubscription subscription,
        ResolvedEvent re,
        int? retryCount,
        CancellationToken ct
    )
    {
        if (re.OriginalEvent.EventStreamId.StartsWith("$"))
            await subscription.Ack(re);

        var context = CreateContext(re, ct)
            .WithItem(ResolvedEventKey, re)
            .WithItem(SubscriptionKey, subscription);

        try
        {
            await Handler(context).ConfigureAwait(false);
            LastProcessed = EventPosition.FromContext(context);
            await Ack(context).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            await Nack(context, e).ConfigureAwait(false);
        }
    }

    private void HandleDrop(
       PersistentSubscription __,
       SubscriptionDroppedReason reason,
       Exception exception
   )
    {
        Dropped(EsdbMappings.AsDropReason(reason), exception);
    }

    IMessageConsumeContext CreateContext(ResolvedEvent re, CancellationToken cancellationToken)
    {
        var evt = DeserializeData(
            re.Event.EventType,
            re.Event.Data,
            re.OriginalStreamId,
            re.Event.Position.CommitPosition
        );

        return new MessageConsumeContext(
            re.Event.EventId.ToString(),
            re.Event.EventType,
            re.Event.ContentType,
            re.OriginalStreamId,
            GetContextStreamPosition(re),
            re.Event.Position.CommitPosition,
            re.OriginalEventNumber,
            re.Event.Created,
            evt,
            MetadataSerializer.Deserialize(
                re.Event.Metadata,
                re.OriginalStreamId,
                re.Event.EventNumber
            ),
            SubscriptionId,
            cancellationToken
        );
    }


    private ConcurrentQueue<ResolvedEvent> AckQueue { get; } = new();

    async Task Ack(IMessageConsumeContext ctx)
    {
        var re = ctx.Items.GetItem<ResolvedEvent>(ResolvedEventKey);
        AckQueue.Enqueue(re);

        if (AckQueue.Count < Options.BufferSize) return;

        var subscription = ctx.Items.GetItem<PersistentSubscription>(SubscriptionKey)!;

        var toAck = new List<ResolvedEvent>();

        for (var i = 0; i < Options.BufferSize; i++)
        {
            if (AckQueue.TryDequeue(out var evt)) toAck.Add(evt);
        }

        await subscription.Ack(toAck).ConfigureAwait(false);
    }

    async Task Nack(IMessageConsumeContext ctx, Exception exception)
    {
        _logger.LogError($"Message Handling Failed: {Options.SubscriptionId}", ctx, exception);

        var re = ctx.Items.GetItem<ResolvedEvent>(ResolvedEventKey);
        var subscription = ctx.Items.GetItem<PersistentSubscription>(SubscriptionKey)!;
        await DefaultEventProcessingFailureHandler(subscription, re, exception)
            .ConfigureAwait(false);
    }

    static Task DefaultEventProcessingFailureHandler(
        PersistentSubscription subscription,
        ResolvedEvent resolvedEvent,
        Exception exception
    )
        => subscription.Nack(
            PersistentSubscriptionNakEventAction.Retry,
            exception.Message,
            resolvedEvent
        );

    protected object DeserializeData(
        string eventType,
        ReadOnlyMemory<byte> data,
        string stream,
        ulong position = 0
    )
    {
        try
        {
            var type = Mapper.GetType(eventType);
            var dataStr = Encoding.UTF8.GetString(data.Span);
            var result = EventSerializer.Deserialize(dataStr, type);
            return result;
        }
        catch (Exception e)
        {
            throw new DeserializationException(stream, eventType, position, e);
        }
    }

    protected abstract ulong GetContextStreamPosition(ResolvedEvent re);

    protected EventPosition? LastProcessed { get; set; }


    protected abstract Task CreatePersistentSubscription(
        PersistentSubscriptionSettings settings,
        CancellationToken cancellationToken
    );

    protected abstract Task<PersistentSubscription> LocalSubscribe(
        Func<PersistentSubscription, ResolvedEvent, int?, CancellationToken, Task> eventAppeared,
        Action<PersistentSubscription, SubscriptionDroppedReason, Exception?>? subscriptionDropped,
        CancellationToken cancellationToken
    );
}