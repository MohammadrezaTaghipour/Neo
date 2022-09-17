using Neo.Specs.ScreenPlay.StreamEventTypes.Commands;
using Suzianna.Core.Screenplay;
using Suzianna.Core.Screenplay.Actors;
using Suzianna.Rest.Screenplay.Interactions;

namespace Neo.Specs.ScreenPlay.StreamEventTypes.Tasks;

public class RemoveStreamEventTypeByApiTask : ITask
{
    public RemoveStreamEventTypeByApiTask(RemoveStreamEventTypeCommand command)
    {
        _command = command;
    }

    private readonly RemoveStreamEventTypeCommand _command;

    public void PerformAs<T>(T actor) where T : Actor
    {
        actor.AttemptsTo(Delete
            .From($"/api/StreamEventTypes/{_command.Id}/{_command.Version}"));
    }
}