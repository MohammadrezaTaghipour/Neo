namespace Neo.Infrastructure.Framework.Subscriptions;

public interface IMessageSubscription
{
    string SubscriptionId { get; }

    Task Subscribe(
        OnSubscribed onSubscribed,
        OnDropped onDropped,
        CancellationToken cancellationToken
    );

    Task Unsubscribe(OnUnsubscribed onUnsubscribed, CancellationToken cancellationToken);
}

public delegate void OnSubscribed(string subscriptionId);

public delegate void OnDropped(string subscriptionId, DropReason dropReason, Exception? exception);

public delegate void OnUnsubscribed(string subscriptionId);

public enum DropReason
{
    Stopped,
    ServerError,
    SubscriptionError
}