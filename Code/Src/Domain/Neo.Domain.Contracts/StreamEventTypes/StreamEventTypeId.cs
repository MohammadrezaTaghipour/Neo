namespace Neo.Domain.Contracts.StreamEventTypes;

using Neo.Infrastructure.Framework.Domain;

public class StreamEventTypeId : AggregateId
{
    public StreamEventTypeId(Guid value) : base(value)
    {
    }

    public static StreamEventTypeId New() => new(Guid.NewGuid());
}