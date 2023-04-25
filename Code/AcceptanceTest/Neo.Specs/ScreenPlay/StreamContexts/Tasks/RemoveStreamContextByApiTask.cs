using Neo.Specs.ScreenPlay.Notifications.Questions;
using Neo.Specs.ScreenPlay.StreamContexts.Commands;
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

        var lastResponse = actor.Recall<LastRequestResponse>();
        if (!lastResponse.HasException())
        {
            var state = actor.AsksFor(
                new GetRequestStatusResponse(lastResponse.RequestId));
            if (state.Completed)
                return;
        }
    }
}
