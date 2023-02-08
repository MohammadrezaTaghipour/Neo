using Neo.Infrastructure.Framework.Domain;

namespace Neo.Domain.Contracts.ReferentialPointers;

public class ReferentialPointerDefined : DomainEvent
{
    public ReferentialPointerId Id { get; }
    public string PointerType { get; }

    public ReferentialPointerDefined(ReferentialPointerId id,
        string pointerType)
    {
        Id = id;
        PointerType = pointerType;
    }
}