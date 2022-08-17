using System.Threading.Tasks;
using Neo.Infrastructure.Framework.Domain;
using Neo.Domain.Contracts.StreamEventTypes;

namespace Neo.Domain.Models.StreamEventTypes;

public class StreamEventType : EventSourcedAggregate<StreamEventTypeState>
{
    private StreamEventType()
    {
    }

    protected StreamEventType(StreamEventTypeArg arg)
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

    public void Modify(StreamEventTypeArg arg)
    {
        Apply(new StreamEventTypeModified(arg.Id, arg.Title));
    }

    public void Remove()
    {
        Apply(new StreamEventTypeRemoved(State.Id));
    }
}