namespace Neo.Domain.Contracts.StreamEventTypes;

using Neo.Infrastructure.Framework.Domain;

public class StreamEventTypeModified : DomainEvent
{
    public StreamEventTypeModified(StreamEventTypeId id,
        string title)
    {
        Id = id;
        Title = title;
    }

    public StreamEventTypeId Id { get; }
    public string Title { get; }
}
