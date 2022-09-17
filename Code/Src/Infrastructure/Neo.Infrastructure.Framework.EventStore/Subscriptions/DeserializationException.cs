namespace Neo.Infrastructure.EventStore.Subscriptions;

public class DeserializationException : Exception
{
    public DeserializationException(string stream, string eventType, ulong position, Exception e)
        : base($"Error deserializing event {stream} {position} {eventType}", e)
    {
    }

    public DeserializationException(string stream, string eventType, ulong position, string message)
        : base($"Error deserializing event {stream} {position} {eventType}: {message}")
    {
    }
}