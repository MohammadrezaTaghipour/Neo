using Neo.Infrastructure.Framework.Domain;

namespace Neo.Infrastructure.Framework.ReferentialPointers;

public class ReferentialPointerId : AggregateId
{
    public ReferentialPointerId(Guid value) : base(value)
    {
    }

    public static ReferentialPointerId New() => new(Guid.NewGuid());
}