namespace Neo.Domain.Contracts.StreamEventTypes;

using Neo.Infrastructure.Framework.Domain;

public class StreamEventTypeModified : DomainEvent
{
    public StreamEventTypeModified(StreamEventTypeId id,
        string title,
        IReadOnlyCollection<StreamEventTypeMetadata> metadata)
    {
        Id = id;
        Title = title;
        Metadata = metadata;
    }

    public StreamEventTypeId Id { get; }
    public string Title { get; }
    public IReadOnlyCollection<StreamEventTypeMetadata> Metadata { get; }

}
