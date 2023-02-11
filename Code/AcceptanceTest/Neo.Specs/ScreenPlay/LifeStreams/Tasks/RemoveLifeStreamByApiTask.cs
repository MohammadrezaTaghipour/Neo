using Neo.Specs.ScreenPlay.LifeStreams.Commands;
using Neo.Specs.ScreenPlay.LifeStreams.Questions;
using Suzianna.Core.Screenplay;
using Suzianna.Core.Screenplay.Actors;
using Suzianna.Rest.Screenplay.Interactions;
using Suzianna.Rest.Screenplay.Questions;

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

        if (!LastResponseException.HasException())
        {
            var status = actor.AsksFor(
                new GetLifeStreamByIdQuestion(_command.Id)).Status;
            if (status.Completed)
                return;
        }
    }
}
