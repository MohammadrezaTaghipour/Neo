using Neo.Specs.ScreenPlay.StreamContexts.Commands;
using Neo.Specs.ScreenPlay.StreamContexts.Questions;
using Suzianna.Core.Screenplay;
using Suzianna.Core.Screenplay.Actors;
using Suzianna.Rest.Screenplay.Interactions;

namespace Neo.Specs.ScreenPlay.StreamContexts.Tasks;

public class ModifyStreamContextByApiTask : ITask
{
    private readonly ModifyStreamContextCommand _command;

    public ModifyStreamContextByApiTask(ModifyStreamContextCommand command)
    {
        _command = command;
    }

    public void PerformAs<T>(T actor) where T : Actor
    {
        var status = actor.AsksFor(new GetStreamContextByIdQuestion(_command.Id)).Status;
        if (status.Completed)
        {
            actor.AttemptsTo(Put.DataAsJson(_command)
                 .To($"/api/StreamContexts/{_command.Id}"));
        }
    }
}
