namespace Neo.Infrastructure.Framework.Notifications;

public interface INotificationPublisher
{
    Task Publish(NotificationMessage message);
}
