using Neo.Domain.Contracts.StreamContexts;
using Neo.Domain.Models.StreamContexts;
using Neo.Infrastructure.Framework.Persistence;

namespace Neo.Infrastructure.Persistence.ES;

public class StreamContextRepository : IStreamContextRepository
{
    private readonly IEventSourcedRepository<StreamContext, StreamContextState, StreamContextId> _repository;

    public StreamContextRepository(
        IEventSourcedRepository<StreamContext, StreamContextState, StreamContextId> repository)
    {
        _repository = repository;
    }

    public async Task<StreamContext> GetBy(StreamContextId id,
       CancellationToken cancellationToken)
         => await _repository.GetById(GetStreamName(id), id, cancellationToken)
            .ConfigureAwait(false);

    public async Task<StreamContext> GetBy(StreamContextId id, long version,
        CancellationToken cancellationToken)
         => await _repository.GetBy(GetStreamName(id),
                id, version, cancellationToken)
            .ConfigureAwait(false);

    public async Task Add(StreamContextId id, StreamContext streamContext,
         CancellationToken cancellationToken)
        => await _repository.Add(GetStreamName(id),
                streamContext, cancellationToken)
            .ConfigureAwait(false);

    static StreamName GetStreamName(StreamContextId id) => StreamName
        .For<StreamContext, StreamContextState, StreamContextId>(id);
}