using Neo.Infrastructure.Framework.Notifications;
using Neo.Infrastructure.Framework.Serializations;

namespace Neo.Infrastructure.Framework.Redis;

public class RedisNotificationPublisher : INotificationPublisher
{
    private readonly IRedisConnectionFactory _connectionFactory;
    private readonly IMessageSerializer _seializer;

    public RedisNotificationPublisher(
        IRedisConnectionFactory connectionFactory,
        IMessageSerializer seializer)
    {
        _connectionFactory = connectionFactory;
        _seializer = seializer;
    }

    public async Task Publish<T>(T message) where T : NotificationMessage
    {
        var db = _connectionFactory.GetConnection().GetDatabase();
        await db.StringSetAsync(message.Id,
            _seializer.Serialize(message),
            message.TTL)
            .ConfigureAwait(false);
    }
}
