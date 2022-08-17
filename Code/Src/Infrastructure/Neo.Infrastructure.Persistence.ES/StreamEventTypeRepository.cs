using Neo.Domain.Contracts.StreamEventTypes;
using Neo.Domain.Models.StreamEventTypes;

namespace Neo.Infrastructure.Persistence.ES;

public class StreamEventTypeRepository : IStreamEventTypeRepository
{
    public Task<StreamEventType> GetBy(StreamEventTypeId id)
    {
        throw new NotImplementedException();
    }

    public Task<StreamEventType> GetBy(StreamEventTypeId id, int version)
    {
        throw new NotImplementedException();
    }

    public Task Add(StreamEventType streamEventType)
    {
        throw new NotImplementedException();
    }
}