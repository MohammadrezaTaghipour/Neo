using Neo.Infrastructure.Framework.Domain;
using Neo.Domain.Contracts.StreamEventTypes;

namespace Neo.Domain.Models.StreamEventTypes;

public interface IStreamEventTypeRepository : IDomainService
{
    Task<StreamEventType> GetBy(StreamEventTypeId id, CancellationToken cancellationToken);
    Task<StreamEventType> GetBy(StreamEventTypeId id, int version, CancellationToken cancellationToken);
    Task Add(StreamEventType streamEventType, CancellationToken cancellationToken);
}