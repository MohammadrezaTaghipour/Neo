using Neo.Infrastructure.Framework.Domain;
using Neo.Domain.Contracts.StreamEventTypes;
using System.Threading.Tasks;

namespace Neo.Domain.Models.StreamEventTypes;

public interface IStreamEventTypeRepository : IDomainService
{
    Task<StreamEventType> GetBy(StreamEventTypeId id);
    Task<StreamEventType> GetBy(StreamEventTypeId id, int version);
    Task Add(StreamEventType streamEventType);
}