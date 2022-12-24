using Neo.Domain.Contracts.LifeStreams;
using Neo.Infrastructure.Framework.Domain;

namespace Neo.Domain.Models.LifeStreams;

public class LifeStream : EventSourcedAggregate<LifeStreamState>
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
        //TODO: check invarinats here :)
        Apply(new LifeStreamRemoved(State.Id));
        return Task.CompletedTask;
    }
}
