using Neo.Specs.ScreenPlay.LifeStreams.Questions;
using Neo.Specs.ScreenPlay.StreamEvents.Commands;
using Suzianna.Core.Screenplay;
using Suzianna.Core.Screenplay.Actors;
using Suzianna.Rest.Screenplay.Interactions;
using Suzianna.Rest.Screenplay.Questions;

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

        if (!actor.Recall<LastResponseException>().HasException())
        {
            var status = actor.AsksFor(
                new GetLifeStreamByIdQuestion(_command.LifeStreamId)).Status;
            if (status.Completed)
                return;
        }
    }
}
