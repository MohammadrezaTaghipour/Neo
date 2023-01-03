using Neo.Domain.Contracts.LifeStreams;
using Neo.Infrastructure.Framework.Domain;

namespace Neo.Domain.Models.LifeStreams;

public record LifeStreamState : AggregateState<LifeStreamState>
{
    public LifeStreamId Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public bool Removed { get; private set; }

    public IReadOnlyCollection<LifeStreamEvent> StreamEvents => _streamEvents.AsReadOnly();
    private List<LifeStreamEvent> _streamEvents = new();


    public override LifeStreamState When(IDomainEvent eventToHandle)
    {
        return When((dynamic)eventToHandle);
    }

    private LifeStreamState When(LifeStreamDefined eventToHandle)
    {
        return this with
        {
            Id = eventToHandle.Id,
            Title = eventToHandle.Title,
            Description = eventToHandle.Description,
        };
    }

    private LifeStreamState When(LifeStreamModified eventToHandle)
    {
        return this with
        {
            Id = eventToHandle.Id,
            Title = eventToHandle.Title,
            Description = eventToHandle.Description,
        };
    }

    private LifeStreamState When(LifeStreamRemoved eventToHandle)
    {
        return this with
        {
            Id = eventToHandle.Id,
            Removed = true
        };
    }

    private LifeStreamState When(LifeStreamEventAppended eventToHandle)
    {
        _streamEvents.Add(new LifeStreamEvent(eventToHandle.Id,
            eventToHandle.LifeStreamId, eventToHandle.StreamContextId,
            eventToHandle.StreamEventTypeId, eventToHandle.Metadata));

        return this;
    }

    private LifeStreamState When(LifeStreamEventRemoved eventToHandled)
    {
        var streamEvent = _streamEvents.Find(_ => _.Id == eventToHandled.Id);
        _streamEvents.Remove(streamEvent);

        return this;
    }
}
