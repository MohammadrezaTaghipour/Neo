using Neo.Domain.Contracts.StreamEventTypes;

namespace Neo.Infrastructure.Projection.MongoDB.StreamEventTypes;

public static class StreamEventTypeViewModelMapper
{
    public static StreamEventTypeProjectionModel MapFrom(StreamEventTypeDefined @event)
    {
        return new StreamEventTypeProjectionModel(@event.Id.Value,
            @event.Title, false, @event.Version,
            @event.Metadata.Select(_ => new StreamEventTypeMetadataProjectionModel(_.Title))
                .ToList());
    }

    public static StreamEventTypeProjectionModel MapFrom(StreamEventTypeModified @event)
    {
        return new StreamEventTypeProjectionModel(@event.Id.Value,
            @event.Title, false, @event.Version,
            @event.Metadata.Select(_ => new StreamEventTypeMetadataProjectionModel(_.Title))
                .ToList());
    }
}