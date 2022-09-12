namespace Neo.Infrastructure.Framework.Subscriptions;

public record SubscriptionOptions
{
    protected SubscriptionOptions(string subscriptionId)
    {
        SubscriptionId = subscriptionId;
    }

    public string SubscriptionId { get; }
}