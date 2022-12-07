using Neo.Specs.ScreenPlay.LifeStreams.Commands;
using Suzianna.Core.Screenplay;
using Suzianna.Core.Screenplay.Actors;
using Suzianna.Rest.Screenplay.Interactions;

namespace Neo.Specs.ScreenPlay.LifeStreams.Tasks;

public class ModifyLifeStreamByApiTask : ITask
{
    private readonly ModifyLifeStreamCommand _command;

    public ModifyLifeStreamByApiTask(ModifyLifeStreamCommand command)
    {
        _command = command;
    }

    public void PerformAs<T>(T actor) where T : Actor
    {
        actor.AttemptsTo(Put.DataAsJson(_command)
            .To($"/api/LifeStreams/{_command.Id}"));
    }
}