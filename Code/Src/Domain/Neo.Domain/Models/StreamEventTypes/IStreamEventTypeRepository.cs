using Neo.Infrastructure.Framework.Domain;
using Neo.Domain.Contracts.StreamEventTypes;

namespace Neo.Domain.Models.StreamEventTypes;

public interface IStreamEventTypeRepository : IDomainRepository
{
    Task<StreamEventType> GetBy(StreamEventTypeId id, CancellationToken cancellationToken);
    Task<StreamEventType> GetBy(StreamEventTypeId id, long version, CancellationToken cancellationToken);
    Task Add(StreamEventTypeId id, StreamEventType streamEventType, CancellationToken cancellationToken);
}