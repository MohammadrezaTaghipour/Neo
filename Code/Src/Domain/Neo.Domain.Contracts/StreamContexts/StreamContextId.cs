using Neo.Domain.Contracts.StreamEventTypes;
using Neo.Infrastructure.Framework.Domain;

namespace Neo.Domain.Contracts.StreamContexts;

public class StreamContextId : AggregateId
{
    public StreamContextId(Guid value) : base(value)
    {
    }

    public static StreamEventTypeId New() => new(Guid.NewGuid());
}