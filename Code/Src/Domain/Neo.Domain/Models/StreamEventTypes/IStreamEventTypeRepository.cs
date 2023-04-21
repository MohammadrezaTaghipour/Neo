using Neo.Domain.Contracts.StreamEventTypes;
using Neo.Infrastructure.Framework.Domain;

namespace Neo.Domain.Models.StreamEventTypes;

public interface IStreamEventTypeRepository : IDomainRepository
{
    Task<StreamEventType> GetBy(StreamEventTypeId id, CancellationToken cancellationToken);
    Task<StreamEventType> GetBy(StreamEventTypeId id, long version, CancellationToken cancellationToken);
    Task Add(StreamEventTypeId id, StreamEventType streamEventType, CancellationToken cancellationToken);
}