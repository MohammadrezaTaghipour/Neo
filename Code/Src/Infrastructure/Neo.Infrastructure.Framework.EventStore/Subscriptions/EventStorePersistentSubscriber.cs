using EventStore.Client;
using Microsoft.Extensions.Logging;
using Neo.Infrastructure.EventStore.Serializations;
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
        IEventSerializer eventSerializer,
        ILoggerFactory loggerFactory)
        : base(subscriptionClient, options, mapper,
            messageConsumer, eventSerializer, loggerFactory)
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
            EventTypeFilter.ExcludeSystemEvents(),
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