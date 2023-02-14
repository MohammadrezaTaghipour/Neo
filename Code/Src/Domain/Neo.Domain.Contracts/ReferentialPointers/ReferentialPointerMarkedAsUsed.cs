using Neo.Infrastructure.Framework.Domain;

namespace Neo.Domain.Contracts.ReferentialPointers;

public class ReferentialPointerMarkedAsUsed : DomainEvent
{
    public ReferentialPointerId Id { get; }
    public string PointerType { get; }

    public ReferentialPointerMarkedAsUsed(ReferentialPointerId id,
        string pointerType)
    {
        Id = id;
        PointerType = pointerType;
    }
}