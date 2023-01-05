using Neo.Infrastructure.Framework.Domain;

namespace Neo.Domain.Contracts.LifeStreams;

public class LifeStreamEventRemoved : DomainEvent
{
    public StreamEventId Id { get; }
    public LifeStreamId LifeStreamId { get; }

    public LifeStreamEventRemoved(StreamEventId id,
        LifeStreamId lifeStreamId)
    {
        Id = id;
        LifeStreamId = lifeStreamId;
    }
}