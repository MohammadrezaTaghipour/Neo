using Neo.Infrastructure.Framework.Domain;
using Neo.Domain.Contracts.StreamEventTypes;

namespace Neo.Domain.Models.StreamEventTypes;

public record StreamEventTypeState : AggregateState<StreamEventTypeState>
{
    public StreamEventTypeId Id { get; private set; }
    public string Title { get; private set; }


    public override StreamEventTypeState When(IDomainEvent eventToHandle)
    {
        return When((dynamic)eventToHandle);
    }

    private StreamEventTypeState When(StreamEventTypeDefined eventToHandle)
    {
        return this with
        {
            Id = eventToHandle.Id,
            Title = eventToHandle.Title
        };
    }

    private StreamEventTypeState When(StreamEventTypeModified eventToHandle)
    {
        return this with
        {
            Id = eventToHandle.Id,
            Title = eventToHandle.Title
        };
    }

    private StreamEventTypeState When(StreamEventTypeRemoved eventToHandle)
    {
        return this with
        {
            Id = eventToHandle.Id
        };
    }
}