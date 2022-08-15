namespace Neo.Domain.Contracts.StreamEventTypes;

using Neo.Infrastructure.Framework.Domain;

public class StreamEventTypeRemoved : DomainEvent
{
    public StreamEventTypeRemoved(StreamEventTypeId id)
    {
        Id = id;
    }

    public StreamEventTypeId Id { get; }
}