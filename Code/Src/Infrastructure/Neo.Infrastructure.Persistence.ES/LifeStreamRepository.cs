using Neo.Domain.Contracts.LifeStreams;
using Neo.Domain.Models.LifeStreams;
using Neo.Infrastructure.Framework.Persistence;

namespace Neo.Infrastructure.Persistence.ES;

public class LifeStreamRepository : ILifeStreamRepository
{
    private readonly IEventSourcedRepository<LifeStream, LifeStreamState, LifeStreamId> _repository;

    public LifeStreamRepository(
        IEventSourcedRepository<LifeStream, LifeStreamState, LifeStreamId> repository)
    {
        _repository = repository;
    }        

    public async Task<LifeStream> GetBy(LifeStreamId id,
       CancellationToken cancellationToken)
         => await _repository.GetById(GetStreamName(id), id, cancellationToken)
            .ConfigureAwait(false);

    public async Task<LifeStream> GetBy(LifeStreamId id, long version, 
        CancellationToken cancellationToken)
         => await _repository.GetBy(GetStreamName(id),
                id, version, cancellationToken)
            .ConfigureAwait(false);

    public async Task Add(LifeStreamId id, LifeStream lifeStream,
         CancellationToken cancellationToken)
        => await _repository.Add(GetStreamName(id),
                lifeStream, cancellationToken)
            .ConfigureAwait(false);

    static StreamName GetStreamName(LifeStreamId id) => StreamName
        .For<LifeStream, LifeStreamState, LifeStreamId>(id);
}