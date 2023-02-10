using Neo.Specs.ScreenPlay.StreamContexts.Commands;
using Neo.Specs.ScreenPlay.StreamContexts.Questions;
using Suzianna.Core.Screenplay;
using Suzianna.Core.Screenplay.Actors;
using Suzianna.Rest.Screenplay.Interactions;

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
        var status = actor.AsksFor(new GetStreamContextByIdQuestion(_command.Id)).Status;
        if (status.Completed)
        {
            actor.AttemptsTo(Delete
                 .From($"/api/StreamContexts/{_command.Id}/{_command.Version}"));
        }
    }
}
