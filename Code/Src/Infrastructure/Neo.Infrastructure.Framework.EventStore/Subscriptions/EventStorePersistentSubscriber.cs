using EventStore.Client;
using Microsoft.Extensions.Logging;
using Neo.Infrastructure.Framework.Subscriptions.Consumers;

namespace Neo.Infrastructure.EventStore.Subscriptions;

public class EventStorePersistentSubscriber :
    PersistentSubscriptionBase<PersistentSubscriptionOptions>
{
    public EventStorePersistentSubscriber(
        EventStorePersistentSubscriptionsClient subscriptionClient,
        PersistentSubscriptionOptions options,
        DomainEventTypeMapper mapper,
        IMessageConsumer messageConsumer,
        ILoggerFactory loggerFactory)
        : base(subscriptionClient, options, mapper, messageConsumer, loggerFactory)
    {
    }

    public EventStorePersistentSubscriber(
        EventStorePersistentSubscriptionsClient subscriptionClient,
        string subscriptionId,
        PersistentSubscriptionOptions options,
        DomainEventTypeMapper mapper,
        IMessageConsumer messageConsumer,
        ILoggerFactory loggerFactory)
        : this(subscriptionClient,
            new PersistentSubscriptionOptions(subscriptionId)
            {
                ResolveLinkTos = true,
            },
            mapper, messageConsumer, loggerFactory)
    {
    }

    protected override ulong GetContextStreamPosition(ResolvedEvent re)
        => re.Event.Position.CommitPosition;

    protected override async Task CreatePersistentSubscription(
        PersistentSubscriptionSettings settings,
        CancellationToken cancellationToken)
    {
        await SubscriptionClient.CreateToAllAsync(
            Options.SubscriptionId,
            settings,
            Options.Deadline,
            Options.Credentials,
            cancellationToken
        );
    }

    protected override Task<PersistentSubscription> LocalSubscribe(
        Func<PersistentSubscription, ResolvedEvent, int?, CancellationToken, Task> eventAppeared,
        Action<PersistentSubscription, SubscriptionDroppedReason, Exception> subscriptionDropped,
        CancellationToken cancellationToken)
    {
        return SubscriptionClient.SubscribeToAllAsync(
            Options.SubscriptionId,
            eventAppeared,
            subscriptionDropped,
            Options.Credentials,
            Options.BufferSize,
            cancellationToken
        );
    }
}