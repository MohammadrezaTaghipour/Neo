using Neo.Infrastructure.Framework.Domain;

namespace Neo.Domain.Contracts.ReferentialPointers;

public class ReferentialPointerRemoved : DomainEvent
{
    public ReferentialPointerId Id { get; }
    public string PointerType { get; }

    public ReferentialPointerRemoved(ReferentialPointerId id,
        string pointerType)
    {
        Id = id;
        PointerType = pointerType;
    }
}