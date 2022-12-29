using Neo.Domain.Contracts.StreamContexts;
using Neo.Domain.Contracts.StreamEventTypes;

namespace Neo.Domain.Contracts.LifeStreams;

public record LifeStreamEvent(long Id, LifeStreamId lifeStreamId,
       StreamContextId StreamContextId, StreamEventTypeId streamEventTypeId,
       IReadOnlyCollection<LifeStreamEventMetada> Metadata);