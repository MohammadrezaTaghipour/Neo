
using Neo.Infrastructure.Framework.Application;
using Neo.Infrastructure.Framework.Redis;
using Neo.Infrastructure.Framework.Serializations;

namespace Neo.Application.Query.Notifications;

public interface INotificationQueryService : IQueryService
{
    Task<RequestStatusResponse?> GetRequestStatus(string requestId,
        CancellationToken cancellationToken);
}


public class NotificationQueryService : INotificationQueryService
{
    private readonly IRedisConnectionFactory _connectionFactory;
    private readonly IMessageSerializer _seializer;

    public NotificationQueryService(
        IRedisConnectionFactory connectionFactory,
        IMessageSerializer seializer)
    {
        _connectionFactory = connectionFactory;
        _seializer = seializer;
    }

    public async Task<RequestStatusResponse?> GetRequestStatus(
        string requestId, CancellationToken cancellationToken)
    {
        var db = _connectionFactory.GetConnection().GetDatabase();
        var value = await db.StringGetAsync(requestId);
        if (!value.HasValue)
            return null;
        return _seializer.Deserialize<RequestStatusResponse>(value);
    }
}