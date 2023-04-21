using Neo.Domain.Contracts.StreamEventTypes;
using Neo.Domain.Models.ReferentialPointers;
using Neo.Infrastructure.Framework.Domain;

namespace Neo.Domain.Models.StreamEventTypes;

public interface IStreamEventType
{
    StreamEventTypeId GetId();
    string GetTitle();
    bool IsRemoved();
}

public class StreamEventType :
    EventSourcedAggregate<StreamEventTypeState>,
    IStreamEventType
{
    private StreamEventType()
    {
    }

    private StreamEventType(StreamEventTypeArg arg)
    {
        Apply(new StreamEventTypeDefined(arg.Id, arg.Title, arg.Metadata));
    }

    public static async Task<StreamEventType> Create(
        StreamEventTypeArg arg)
    {
        StreamEventType streamEventType = new(arg);
        await (Task)streamEventType.CompletionTask;
        return streamEventType;
    }

    public StreamEventTypeId GetId()
    {
        return State.Id;
    }

    public string GetTitle()
    {
        return State.Title;
    }

    public bool IsRemoved()
    {
        return State.Removed;
    }

    public Task Modify(StreamEventTypeArg arg)
    {
        Apply(new StreamEventTypeModified(arg.Id, arg.Title, arg.Metadata));
        return Task.CompletedTask;
    }

    public Task Remove(IReferentialPointer referentialPointer)
    {
        GuardAgainstReferentialIntegrity(referentialPointer);
        Apply(new StreamEventTypeRemoved(State.Id));
        return Task.CompletedTask;
    }

    static void GuardAgainstReferentialIntegrity(
        IReferentialPointer referentialPointer)
    {
        if (referentialPointer != null && referentialPointer.GetCounter() > 0)
            throw new BusinessException(StreamEventTypeErrorCodes.SET_BR_10010);
    }
}