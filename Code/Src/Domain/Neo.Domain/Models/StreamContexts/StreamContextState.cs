using Neo.Domain.Contracts.StreamContexts;
using Neo.Infrastructure.Framework.Domain;

namespace Neo.Domain.Models.StreamContexts;

public record StreamContextState : AggregateState<StreamContextState>
{
    private List<Guid> _streamEventTypes = new();

    public StreamContextId Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public bool Removed { get; private set; }
    public IReadOnlyCollection<Guid> StreamEventTypes => _streamEventTypes.AsReadOnly();

    public override StreamContextState When(IDomainEvent eventToHandle)
    {
        return When((dynamic)eventToHandle);
    }

    private StreamContextState When(StreamContextDefined eventToHandle)
    {
        return this with
        {
            Id = eventToHandle.Id,
            Title = eventToHandle.Title,
            Description = eventToHandle.Description,
            _streamEventTypes = eventToHandle.StreamEventTypes.Select(_ => _.Value).ToList(),
        };
    }

    private StreamContextState When(StreamContextModified eventToHandle)
    {
        return this with
        {
            Id = eventToHandle.Id,
            Title = eventToHandle.Title,
            Description = eventToHandle.Description,
            _streamEventTypes = eventToHandle.StreamEventTypes.Select(_ => _.Value).ToList(),
        };
    }

    private StreamContextState When(StreamContextRemoved eventToHandle)
    {
        return this with
        {
            Id = eventToHandle.Id,
            Removed = true
        };
    }
}