using Neo.Specs.ScreenPlay.LifeStreams.Commands;
using Neo.Specs.ScreenPlay.LifeStreams.Questions;
using Suzianna.Core.Screenplay;
using Suzianna.Core.Screenplay.Actors;
using Suzianna.Rest.Screenplay.Interactions;
using Suzianna.Rest.Screenplay.Questions;

namespace Neo.Specs.ScreenPlay.LifeStreams.Tasks;

public class DefineLifeStreamByApiTask : ITask
{
    private readonly DefineLifeStreamCommand _command;

    public DefineLifeStreamByApiTask(DefineLifeStreamCommand command)
    {
        _command = command;
    }

    public void PerformAs<T>(T actor) where T : Actor
    {
        actor.AttemptsTo(Post.DataAsJson(_command)
            .To($"/api/LifeStreams"));

        if (!actor.Recall<LastRequestResponse>().HasException())
        {
            var status = actor.AsksFor(
                new GetLifeStreamByIdQuestion(_command.Id)).Status;
            if (status.Completed)
                return;
        }
    }
}
