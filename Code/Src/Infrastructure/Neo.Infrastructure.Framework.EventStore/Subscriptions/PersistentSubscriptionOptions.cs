using EventStore.Client;
using Neo.Infrastructure.Framework.Subscriptions;

namespace Neo.Infrastructure.EventStore.Subscriptions;

public record PersistentSubscriptionOptions : SubscriptionOptions
{
    public PersistentSubscriptionOptions(string subscriptionId)
        : base(subscriptionId)
    {
    }

    public UserCredentials Credentials { get; set; }
    public bool ResolveLinkTos { get; set; }
    public PersistentSubscriptionSettings SubscriptionSettings { get; set; }
    public int BufferSize { get; set; } = 10;

    public TimeSpan? Deadline { get; set; }
}