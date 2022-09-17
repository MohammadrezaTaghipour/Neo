using System.Threading.Tasks;
using Neo.Infrastructure.Framework.Domain;
using Neo.Domain.Contracts.StreamEventTypes;

namespace Neo.Domain.Models.StreamEventTypes;

public class StreamEventType : EventSourcedAggregate<StreamEventTypeState>
{
    private StreamEventType()
    {
    }

    private StreamEventType(StreamEventTypeArg arg)
    {
        Apply(new StreamEventTypeDefined(arg.Id, arg.Title));
    }

    public static async Task<StreamEventType> Create(
        StreamEventTypeArg arg)
    {
        StreamEventType streamEventType = new(arg);
        await (Task)streamEventType.CompletionTask;
        return streamEventType;
    }

    public async Task Modify(StreamEventTypeArg arg)
    {
        Apply(new StreamEventTypeModified(arg.Id, arg.Title));
    }

    public async Task Remove()
    {
        Apply(new StreamEventTypeRemoved(State.Id));
    }
}