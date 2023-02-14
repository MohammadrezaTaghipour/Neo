using Neo.Specs.ScreenPlay.StreamEventTypes.Commands;
using Neo.Specs.ScreenPlay.StreamEventTypes.Questions;
using Suzianna.Core.Screenplay;
using Suzianna.Core.Screenplay.Actors;
using Suzianna.Rest.Screenplay.Interactions;
using Suzianna.Rest.Screenplay.Questions;

namespace Neo.Specs.ScreenPlay.StreamEventTypes.Tasks;

public class DefineStreamEventTypeByApiTask : ITask
{
    private readonly DefineStreamEventTypeCommand _command;

    public DefineStreamEventTypeByApiTask(DefineStreamEventTypeCommand command)
    {
        _command = command;
    }

    public void PerformAs<T>(T actor) where T : Actor
    {
        actor.AttemptsTo(Post.DataAsJson(_command)
            .To($"/api/StreamEventTypes"));

        if (!actor.Recall<LastResponseException>().HasException())
        {
            var state = actor.AsksFor(new GetStreamEventTypeByIdQuestion(_command.Id)).Status;
            if (state.Completed)
                return;
        }
    }
}