using Neo.Specs.ScreenPlay.LifeStreams.Commands;
using Suzianna.Core.Screenplay;
using Suzianna.Core.Screenplay.Actors;
using Suzianna.Rest.Screenplay.Interactions;

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
    }
}
