using Neo.Domain.Contracts.StreamEventTypes;
using Neo.Infrastructure.Framework.Domain;

namespace Neo.Domain.Contracts.StreamContexts;

public class StreamContextModified: DomainEvent
{
    public StreamContextModified(StreamContextId id,
    string title, string description,
    IReadOnlyCollection<StreamEventTypeId> streamEventTypes)
    {
        Id = id;
        Title = title;
        Description = description;
        StreamEventTypes = streamEventTypes;
    }

    public StreamContextId Id { get; }
    public string Title { get; }
    public string Description { get; }
    public IReadOnlyCollection<StreamEventTypeId> StreamEventTypes { get; }
}