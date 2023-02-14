using Neo.Domain.Contracts.StreamContexts;
using Neo.Infrastructure.Framework.Domain;

namespace Neo.Domain.Models.StreamContexts;

public interface IStreamContextRepository : IDomainRepository
{
    Task<StreamContext> GetBy(StreamContextId id, CancellationToken cancellationToken);
    Task<StreamContext> GetBy(StreamContextId id, long version, CancellationToken cancellationToken);
    Task Add(StreamContextId id, StreamContext lifeStream, CancellationToken cancellationToken);
}
