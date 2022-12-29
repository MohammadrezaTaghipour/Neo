using Neo.Domain.Contracts.StreamContexts;
using Neo.Domain.Contracts.StreamEventTypes;
using Neo.Infrastructure.Framework.Domain;

namespace Neo.Domain.Contracts.LifeStreams;

public class LifeStreamEventAppended : DomainEvent
{
    public LifeStreamEventAppended(long id,
        LifeStreamId lifeStreamId, StreamContextId streamContextId,
        StreamEventTypeId streamEventTypeId,
       IReadOnlyCollection<LifeStreamEventMetada> metadata)
    {
        Id = id;
        LifeStreamId = lifeStreamId;
        StreamContextId = streamContextId;
        StreamEventTypeId = streamEventTypeId;
        Metadata = metadata;
    }

    public long Id { get; }
    public LifeStreamId LifeStreamId { get; }
    public StreamContextId StreamContextId { get; }
    public StreamEventTypeId StreamEventTypeId { get; }
    public IReadOnlyCollection<LifeStreamEventMetada> Metadata { get; }

}