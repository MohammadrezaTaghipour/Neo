using Neo.Domain.Contracts.LifeStreams;
using Neo.Infrastructure.Framework.Domain;

namespace Neo.Domain.Models.LifeStreams;

public interface ILifeStreamRepository : IDomainRepository
{
    Task<LifeStream> GetBy(LifeStreamId id, CancellationToken cancellationToken);
    Task<LifeStream> GetBy(LifeStreamId id, long version, CancellationToken cancellationToken);
    Task Add(LifeStreamId id, LifeStream lifeStream, CancellationToken cancellationToken);
}