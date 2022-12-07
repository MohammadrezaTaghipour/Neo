using Neo.Specs.Framework;
using Neo.Specs.ScreenPlay.LifeStreams.Commands;
using Suzianna.Core.Screenplay;

namespace Neo.Specs.ScreenPlay.LifeStreams.Tasks;

public class LifeStreamRestApiCommandHandler :
    ICommandHandler<DefineLifeStreamCommand>,
    ICommandHandler<ModifyLifeStreamCommand>,
    ICommandHandler<RemoveLifeStreamCommand>
{
    public ITask Handle(DefineLifeStreamCommand command)
        => new DefineLifeStreamByApiTask(command);

    public ITask Handle(ModifyLifeStreamCommand command)
        => new ModifyLifeStreamByApiTask(command);

    public ITask Handle(RemoveLifeStreamCommand command)
        => new RemoveLifeStreamByApiTask(command);
}