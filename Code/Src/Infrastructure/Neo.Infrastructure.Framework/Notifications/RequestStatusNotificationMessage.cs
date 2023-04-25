
namespace Neo.Infrastructure.Framework.Notifications;

public class RequestStatusNotificationMessage : NotificationMessage
{
    public Guid? EntityId { get; }
    public string? State { get; }
    public bool Completed { get; }
    public string? ErrorCode { get; }
    public string? ErrorMessage { get; }

    private RequestStatusNotificationMessage(string requestId,
        Guid? entityId, string? state, bool completed,
        string? errorCode, string? errorMessage,
        TimeSpan ttl)
        : base(requestId, ttl)
    {
        EntityId = entityId;
        State = state;
        Completed = completed;
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }


    public static RequestStatusNotificationMessage Init(string requestId)
        => new(requestId, null, null, false, null, null, TimeSpan.FromSeconds(60));

    public static RequestStatusNotificationMessage Success(string requestId, Guid entityId, string state)
        => new(requestId, entityId, state, true, null, null, TimeSpan.FromSeconds(60));

    public static RequestStatusNotificationMessage Failed(string requestId, Guid entityId, string state,
        string errorCode, string errorMessage)
        => new(requestId, entityId, state, true, errorCode, errorMessage, TimeSpan.FromSeconds(60));
}