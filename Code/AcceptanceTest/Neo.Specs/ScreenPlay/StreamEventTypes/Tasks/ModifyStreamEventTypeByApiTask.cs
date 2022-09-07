using Neo.Specs.ScreenPlay.StreamEventTypes.Commands;
using Suzianna.Core.Screenplay;
using Suzianna.Core.Screenplay.Actors;
using Suzianna.Rest.Screenplay.Interactions;

namespace Neo.Specs.ScreenPlay.StreamEventTypes.Tasks;

public class ModifyStreamEventTypeByApiTask : ITask
{
    public ModifyStreamEventTypeByApiTask(ModifyStreamEventTypeCommand command)
    {
        _command = command;
    }

    private readonly ModifyStreamEventTypeCommand _command;

    public void PerformAs<T>(T actor) where T : Actor
    {
        actor.AttemptsTo(Put.DataAsJson(_command)
            .To($"/api/StreamEventTypes/{_command.Id}"));
    }
}