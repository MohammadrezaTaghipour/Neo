using Neo.Infrastructure.Framework.Domain;

namespace Neo.Domain.Contracts.ReferentialPointers;

public class ReferentialPointerMarkedAsUnused : DomainEvent
{
    public ReferentialPointerId Id { get; }
    public string PointerType { get; }

    public ReferentialPointerMarkedAsUnused(ReferentialPointerId id,
        string pointerType)
    {
        Id = id;
        PointerType = pointerType;
    }
}