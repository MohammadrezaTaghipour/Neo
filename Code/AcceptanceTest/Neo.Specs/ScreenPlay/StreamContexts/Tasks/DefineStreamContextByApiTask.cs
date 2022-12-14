using Neo.Specs.ScreenPlay.StreamContexts.Commands;
using Suzianna.Core.Screenplay;
using Suzianna.Core.Screenplay.Actors;
using Suzianna.Rest.Screenplay.Interactions;

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
    }
}
