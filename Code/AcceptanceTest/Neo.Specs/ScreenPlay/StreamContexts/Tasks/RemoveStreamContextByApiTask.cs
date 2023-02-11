using Neo.Specs.ScreenPlay.StreamContexts.Commands;
using Neo.Specs.ScreenPlay.StreamContexts.Questions;
using Suzianna.Core.Screenplay;
using Suzianna.Core.Screenplay.Actors;
using Suzianna.Rest.Screenplay.Interactions;
using Suzianna.Rest.Screenplay.Questions;

namespace Neo.Specs.ScreenPlay.StreamContexts.Tasks;

public class RemoveStreamContextByApiTask : ITask
{
    private readonly RemoveStreamContextCommand _command;

    public RemoveStreamContextByApiTask(RemoveStreamContextCommand command)
    {
        _command = command;
    }

    public void PerformAs<T>(T actor) where T : Actor
    {
        actor.AttemptsTo(Delete
             .From($"/api/StreamContexts/{_command.Id}/{_command.Version}"));

        if (!LastResponseException.HasException())
        {
            var status = actor.AsksFor(new GetStreamContextByIdQuestion(_command.Id)).Status;
            if (status.Completed)
                return;
        }
    }
}
