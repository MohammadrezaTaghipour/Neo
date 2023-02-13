using Neo.Infrastructure.Framework.Domain;

namespace Neo.Domain.Contracts.ReferentialPointers;

public class ReferentialPointerId : AggregateId
{
    public ReferentialPointerId(Guid value) : base(value)
    {
    }

    public static ReferentialPointerId New() => new(Guid.NewGuid());
}