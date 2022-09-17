namespace Neo.Domain.Contracts.StreamEventTypes;

using Neo.Infrastructure.Framework.Domain;

public class StreamEventTypeDefined : DomainEvent
{
    public StreamEventTypeDefined(StreamEventTypeId id,
        string title)
    {
        Id = id;
        Title = title;
    }

    public StreamEventTypeId Id { get; }
    public string Title { get; }
}
