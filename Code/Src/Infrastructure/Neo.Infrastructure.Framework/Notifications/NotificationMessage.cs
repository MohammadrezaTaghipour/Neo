namespace Neo.Infrastructure.Framework.Notifications;

public abstract class NotificationMessage
{
    public string Id { get; }
    public TimeSpan? TTL { get; }
    public DateTime PublishDateTime { get; }

    protected NotificationMessage(string id, TimeSpan? ttl)
    {
        Id = id;
        TTL = ttl;
        PublishDateTime = DateTime.Now;
    }
}
