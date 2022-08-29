using Neo.Domain.Contracts.StreamEventTypes;
using Neo.Domain.Models.StreamEventTypes;
using Neo.Infrastructure.Framework.Persistence;

namespace Neo.Infrastructure.Persistence.ES;

public class StreamEventTypeRepository : IStreamEventTypeRepository
{
    private IEventSourcedRepository<StreamEventType, StreamEventTypeState, StreamEventTypeId> _repository;

    public StreamEventTypeRepository(
        IEventSourcedRepository<StreamEventType, StreamEventTypeState, StreamEventTypeId> repository)
    {
        _repository = repository;
    }

    public async Task<StreamEventType> GetBy(StreamEventTypeId id,
        CancellationToken cancellationToken)
        => await _repository.GetById(id, cancellationToken)
            .ConfigureAwait(false);

    public async Task<StreamEventType> GetBy(StreamEventTypeId id,
        int version, CancellationToken cancellationToken)
        => await _repository.GetBy(id, version, cancellationToken)
            .ConfigureAwait(false);

    public async Task Add(StreamEventType streamEventType,
        CancellationToken cancellationToken)
        => await _repository.Add(streamEventType, cancellationToken)
            .ConfigureAwait(false);
}