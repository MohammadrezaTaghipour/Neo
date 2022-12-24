using Neo.Infrastructure.Framework.Domain;

namespace Neo.Domain.Contracts.LifeStreams;


public class LifeStreamId : AggregateId
{
    public LifeStreamId(Guid value) : base(value)
    {
    }

    public static LifeStreamId New() => new(Guid.NewGuid());
}
