using EventStore.Client;

namespace Neo.Infrastructure.EventStore.Subscriptions;

public class EventStorePersistentSubscriber :
    PersistentSubscriptionBase<PersistentSubscriptionOptions>
{
    public EventStorePersistentSubscriber(
        EventStoreClient eventStoreClient,
        PersistentSubscriptionOptions options,
        DomainEventTypeMapper mapper)
        : base(eventStoreClient, options, mapper)
    {
    }

    public EventStorePersistentSubscriber(
        EventStoreClient eventStoreClient,
        string subscriptionId,
        PersistentSubscriptionOptions options,
        DomainEventTypeMapper mapper)
        : this(eventStoreClient,
            new PersistentSubscriptionOptions
            {
                SubscriptionId = subscriptionId
            }, mapper)
    {
    }

    protected override ulong GetContextStreamPosition(ResolvedEvent re) => re.Event.Position.CommitPosition;
    
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