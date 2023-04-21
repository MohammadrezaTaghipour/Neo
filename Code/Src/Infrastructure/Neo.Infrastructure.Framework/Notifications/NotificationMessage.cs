namespace Neo.Infrastructure.Framework.Notifications;

public class NotificationMessage
{
    public string Id { get; }
    public object Content { get; }

    public NotificationMessage(string id, object content)
    {
        Id = id;
        Content = content;
    }
}
