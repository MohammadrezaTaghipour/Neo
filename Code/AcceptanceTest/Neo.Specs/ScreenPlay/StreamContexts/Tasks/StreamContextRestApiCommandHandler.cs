using Neo.Specs.Framework;
using Neo.Specs.ScreenPlay.StreamContexts.Commands;
using Suzianna.Core.Screenplay;

namespace Neo.Specs.ScreenPlay.StreamContexts.Tasks;

public class StreamContextRestApiCommandHandler :
    ICommandHandler<DefineStreamContextCommand>,
    ICommandHandler<ModifyStreamContextCommand>,
    ICommandHandler<RemoveStreamContextCommand>
{
    public ITask Handle(DefineStreamContextCommand command)
        => new DefineStreamContextByApiTask(command);

    public ITask Handle(ModifyStreamContextCommand command)
        => new ModifyStreamContextByApiTask(command);

    public ITask Handle(RemoveStreamContextCommand command)
        => new RemoveStreamContextByApiTask(command);
}