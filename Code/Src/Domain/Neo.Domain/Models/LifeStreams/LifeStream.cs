using Neo.Domain.Contracts.LifeStreams;
using Neo.Infrastructure.Framework.Domain;

namespace Neo.Domain.Models.LifeStreams;

public partial class LifeStream : EventSourcedAggregate<LifeStreamState>
{
    private LifeStream() { }

    private LifeStream(LifeStreamArg arg)
    {
        Apply(new LifeStreamDefined(arg.Id,
            arg.Title, arg.Description));
    }

    public static async Task<LifeStream> Create(
        LifeStreamArg arg)
    {
        LifeStream lifeStream = new(arg);
        await (Task)lifeStream.CompletionTask;
        return lifeStream;
    }

    public Task Modify(LifeStreamArg arg)
    {
        Apply(new LifeStreamModified(arg.Id,
            arg.Title, arg.Description));
        return Task.CompletedTask;
    }

    public Task Remove()
    {
        GuardAginstRemovalIfAnyStreamEventsHasBeenAppendedBefore(this);

        Apply(new LifeStreamRemoved(State.Id));
        return Task.CompletedTask;
    }

    public Task AppendStreamEvent(StreamEventArg arg)
    {
        GuardAgainstRemovedLifeStream(this);
        GuardAgainstRemovedStreamContext(arg.StreamContext);
        GuardAgainstRemovedStreamEventType(arg.StreamEventType);
        GuardAgainstStreamEventMetadataToBeBasedOnStreamEventTypeConfiguration(arg);

        Apply(new LifeStreamEventAppended(arg.Id,
            State.Id, arg.StreamContext.GetId(), arg.StreamEventType.GetId(),
            arg.Metadata));
        return Task.CompletedTask;
    }

    public Task RemoveStreamEvent(StreamEventId id)
    {
        Apply(new LifeStreamEventRemoved(id, State.Id));
        return Task.CompletedTask;
    }
}
