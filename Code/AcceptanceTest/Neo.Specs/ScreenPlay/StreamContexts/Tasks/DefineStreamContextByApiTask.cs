using Neo.Specs.ScreenPlay.StreamContexts.Commands;
using Neo.Specs.ScreenPlay.StreamContexts.Questions;
using Suzianna.Core.Screenplay;
using Suzianna.Core.Screenplay.Actors;
using Suzianna.Rest.Screenplay.Interactions;
using Suzianna.Rest.Screenplay.Questions;

namespace Neo.Specs.ScreenPlay.StreamContexts.Tasks;

public class DefineStreamContextByApiTask : ITask
{
    private readonly DefineStreamContextCommand _command;

    public DefineStreamContextByApiTask(DefineStreamContextCommand command)
    {
        _command = command;
    }

    public void PerformAs<T>(T actor) where T : Actor
    {
        actor.AttemptsTo(Post.DataAsJson(_command)
             .To($"/api/StreamContexts"));

        if (!actor.Recall<LastResponseException>().HasException())
        {
            var status = actor.AsksFor(
                new GetStreamContextByIdQuestion(_command.Id)).Status;
            if (status.Completed)
                return;
        }
    }
}
