using Neo.Domain.Contracts.StreamContexts;
using Neo.Domain.Models.ReferentialPointers;
using Neo.Domain.Models.StreamEventTypes;
using Neo.Infrastructure.Framework.Domain;

namespace Neo.Domain.Models.StreamContexts;

public interface IStreamContext
{
    StreamContextId GetId();
    bool IsRemoved();
}

public class StreamContext : EventSourcedAggregate<StreamContextState>,
    IStreamContext
{
    private StreamContext() { }

    private StreamContext(StreamContextArg arg)
    {
        GuardAgainstRemovedStreamEventTypes(arg.StreamEventTypes);

        Apply(new StreamContextDefined(arg.Id,
            arg.Title, arg.Description,
            arg.StreamEventTypes.Select(_ => _.GetId()).ToList()));
    }

    public static async Task<StreamContext> Create(
        StreamContextArg arg)
    {
        StreamContext streamContext = new(arg);
        await (Task)streamContext.CompletionTask;
        return streamContext;
    }

    public Task Modify(StreamContextArg arg)
    {
        GuardAgainstRemovedStreamEventTypes(arg.StreamEventTypes);

        Apply(new StreamContextModified(arg.Id,
            arg.Title, arg.Description,
            arg.StreamEventTypes.Select(_ => _.GetId()).ToList()));
        return Task.CompletedTask;
    }

    public Task Remove(IReferentialPointer referentialPointer)
    {
        GuardAgainstReferentialIntegrity(State.Id, referentialPointer);
        Apply(new StreamContextRemoved(State.Id));
        return Task.CompletedTask;
    }

    public StreamContextId GetId()
    {
        return State.Id;
    }

    public bool IsRemoved()
    {
        return State.Removed;
    }

    static void GuardAgainstRemovedStreamEventTypes(
        IReadOnlyCollection<IStreamEventType> streamEventTypes)
    {
        if (streamEventTypes.Any(_ => _.IsRemoved()))
            throw new BusinessException(StreamContextErrorCodes.SC_BR_10007);
    }

    static void GuardAgainstReferentialIntegrity(StreamContextId id,
        IReferentialPointer referentialPointer)
    {
        if (referentialPointer != null && referentialPointer.GetCounter() > 0)
            throw new BusinessException(StreamContextErrorCodes.SC_BR_10009);
    }
}