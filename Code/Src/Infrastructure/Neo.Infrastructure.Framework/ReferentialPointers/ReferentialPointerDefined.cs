using Neo.Infrastructure.Framework.Domain;

namespace Neo.Infrastructure.Framework.ReferentialPointers;

public class ReferentialPointerDefined : DomainEvent
{
    public ReferentialPointerId Id { get; }
    public string PointerType { get; set; }

    public ReferentialPointerDefined(ReferentialPointerId id,
        string pointerType)
    {
        Id = id; 
        PointerType = pointerType;
    }
}