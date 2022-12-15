using Neo.Domain.Contracts.StreamContexts;
using Neo.Infrastructure.Framework.Domain;

namespace Neo.Domain.Models.StreamContexts;

public class StreamContext : EventSourcedAggregate<StreamContextState>
{
    private StreamContext() { }

    private StreamContext(StreamContextArg arg)
    {
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
}