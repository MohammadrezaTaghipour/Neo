namespace Neo.Domain.Models.StreamEventTypes;

using Neo.Infrastructure.Framework.Domain;
using Neo.Domain.Contracts.StreamEventTypes;

public record StreamEventTypeState : AggregateState<StreamEventTypeState>
{

    public StreamEventTypeId Id { get; set; }

    public override StreamEventTypeState When(IDomainEvent eventToHandle)
    {
        return When((dynamic)eventToHandle);
    }

    private StreamEventTypeState When(StreamEventTypeDefined eventToHandle)
    {
        return this with
        {
            Id = eventToHandle.Id
        };
    }

    private StreamEventTypeState When(StreamEventTypeModified eventToHandle)
    {
        return this with
        {
            Id = eventToHandle.Id
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