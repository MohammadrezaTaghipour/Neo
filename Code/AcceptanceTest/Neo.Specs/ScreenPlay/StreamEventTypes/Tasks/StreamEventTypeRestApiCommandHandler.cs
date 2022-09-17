using Neo.Specs.Framework;
using Neo.Specs.ScreenPlay.StreamEventTypes.Commands;
using Suzianna.Core.Screenplay;

namespace Neo.Specs.ScreenPlay.StreamEventTypes.Tasks;

public class StreamEventTypeRestApiCommandHandler :
    ICommandHandler<DefineStreamEventTypeCommand>,
    ICommandHandler<ModifyStreamEventTypeCommand>,
    ICommandHandler<RemoveStreamEventTypeCommand>
{
    public ITask Handle(DefineStreamEventTypeCommand command)
        => new DefineStreamEventTypeByApiTask(command);

    public ITask Handle(ModifyStreamEventTypeCommand command)
        => new ModifyStreamEventTypeByApiTask(command);

    public ITask Handle(RemoveStreamEventTypeCommand command)
        => new RemoveStreamEventTypeByApiTask(command);
}