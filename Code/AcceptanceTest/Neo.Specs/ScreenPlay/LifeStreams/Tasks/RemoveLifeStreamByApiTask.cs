using Neo.Specs.ScreenPlay.LifeStreams.Commands;
using Suzianna.Core.Screenplay;
using Suzianna.Core.Screenplay.Actors;
using Suzianna.Rest.Screenplay.Interactions;

namespace Neo.Specs.ScreenPlay.LifeStreams.Tasks;

public class RemoveLifeStreamByApiTask : ITask
{
    private readonly RemoveLifeStreamCommand _command;

    public RemoveLifeStreamByApiTask(RemoveLifeStreamCommand command)
    {
        _command = command;
    }

    public void PerformAs<T>(T actor) where T : Actor
    {
        actor.AttemptsTo(Delete
          .From($"/api/LifeStreams/{_command.Id}/{_command.Version}"));
    }
}
