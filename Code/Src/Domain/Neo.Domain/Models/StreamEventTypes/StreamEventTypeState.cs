using Neo.Domain.Contracts.StreamEventTypes;
using Neo.Infrastructure.Framework.Domain;

namespace Neo.Domain.Models.StreamEventTypes;

public record StreamEventTypeState : AggregateState<StreamEventTypeState>
{
    private List<StreamEventTypeMetadata> _metadata = new();

    public StreamEventTypeId Id { get; private set; }
    public string Title { get; private set; }
    public bool Removed { get; private set; }
    public IReadOnlyCollection<StreamEventTypeMetadata> Metadata => _metadata.AsReadOnly();


    public override StreamEventTypeState When(IDomainEvent eventToHandle)
    {
        return When((dynamic)eventToHandle);
    }

    private StreamEventTypeState When(StreamEventTypeDefined eventToHandle)
    {
        return this with
        {
            Id = eventToHandle.Id,
            Title = eventToHandle.Title,
            _metadata = eventToHandle.Metadata?.ToList(),
        };
    }

    private StreamEventTypeState When(StreamEventTypeModified eventToHandle)
    {
        return this with
        {
            Id = eventToHandle.Id,
            Title = eventToHandle.Title,
            _metadata = eventToHandle.Metadata?.ToList()
        };
    }

    private StreamEventTypeState When(StreamEventTypeRemoved eventToHandle)
    {
        return this with
        {
            Id = eventToHandle.Id,
            Removed = true
        };
    }
}