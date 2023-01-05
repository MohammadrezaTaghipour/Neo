using Neo.Specs.ScreenPlay.StreamEvents.Commands;
using Suzianna.Core.Screenplay;
using Suzianna.Core.Screenplay.Actors;
using Suzianna.Rest.Screenplay.Interactions;

namespace Neo.Specs.ScreenPlay.StreamEvents.Tasks;

public class PartialModifyLifeStreamByApiTask : ITask
{
    private readonly PartialModifyLifeStreamCommand _command;

    public PartialModifyLifeStreamByApiTask(PartialModifyLifeStreamCommand command)
    {
        _command = command;
    }

    public void PerformAs<T>(T actor) where T : Actor
    {
        actor.AttemptsTo(Patch.DataAsJson(_command)
            .To($"/api/LifeStreams/{_command.LifeStreamId}"));
    }
}
