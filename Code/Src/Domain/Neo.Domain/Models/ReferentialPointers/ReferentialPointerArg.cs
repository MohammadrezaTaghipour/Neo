using Neo.Domain.Contracts.ReferentialPointers;

namespace Neo.Domain.Models.ReferentialPointers;

public class ReferentialPointerArg
{
    public ReferentialPointerId Id { get; set; }
    public string PointerType { get; set; }
}