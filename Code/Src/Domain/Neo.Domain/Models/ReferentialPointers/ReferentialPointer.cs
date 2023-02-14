using Neo.Domain.Contracts.ReferentialPointers;
using Neo.Infrastructure.Framework.Domain;

namespace Neo.Domain.Models.ReferentialPointers;

public interface IReferentialPointer
{
    int GetCounter();
}

public class ReferentialPointer :
    EventSourcedAggregate<ReferentialPointerState>,
    IReferentialPointer
{
    private ReferentialPointer()
    {
    }

    private ReferentialPointer(ReferentialPointerArg arg)
    {
        Apply(new ReferentialPointerDefined(arg.Id,
            arg.PointerType.ToLower()));
    }

    public static async Task<ReferentialPointer> Create(ReferentialPointerArg arg)
    {
        var refPointer = new ReferentialPointer(arg);
        await (Task)refPointer.CompletionTask;
        return refPointer;
    }

    public bool IsRemoved => State.IsRemoved;

    public void MarkAsUsed(ReferentialPointerArg arg)
    {
        Apply(new ReferentialPointerMarkedAsUsed(arg.Id,
            arg.PointerType.ToLower()));
    }

    public void MarkAsUnused(ReferentialPointerArg arg)
    {
        Apply(new ReferentialPointerMarkedAsUnused(arg.Id,
            arg.PointerType.ToLower()));
    }

    public void Remove(ReferentialPointerArg arg)
    {
        if (State.Counter > 0)
            throw new ReferentialPointerCantBeRemovedDueToItsUsage();
        Apply(new ReferentialPointerRemoved(arg.Id,
            arg.PointerType.ToLower()));
    }

    public int GetCounter()
    {
        return State.Counter;
    }
}
