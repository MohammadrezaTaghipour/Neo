using Neo.Domain.Contracts.StreamContexts;
using Neo.Domain.Models.StreamEventTypes;
using Neo.Infrastructure.Framework.Domain;

namespace Neo.Domain.Models.StreamContexts;

public class StreamContext : EventSourcedAggregate<StreamContextState>
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

    public Task Remove()
    {
        //Todo: add invariants here :)
        Apply(new StreamContextRemoved(State.Id));
        return Task.CompletedTask;
    }

    static void GuardAgainstRemovedStreamEventTypes(
        IReadOnlyCollection<IStreamEventType> streamEventTypes)
    {
        if (streamEventTypes.Any(_ => _.IsRemoved()))
            throw new BusinessException(StreamContextErrorCodes.SC_BR_10007);
    }
}