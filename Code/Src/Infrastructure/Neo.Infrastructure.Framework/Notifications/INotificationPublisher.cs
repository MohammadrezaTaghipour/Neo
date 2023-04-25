namespace Neo.Infrastructure.Framework.Notifications;

public interface INotificationPublisher
{
    Task Publish<T>(T message) where T : NotificationMessage;
}
