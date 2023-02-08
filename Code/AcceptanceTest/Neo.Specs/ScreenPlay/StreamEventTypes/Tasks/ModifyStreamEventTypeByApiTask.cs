using Neo.Specs.ScreenPlay.StreamEventTypes.Commands;
using Neo.Specs.ScreenPlay.StreamEventTypes.Questions;
using Suzianna.Core.Screenplay;
using Suzianna.Core.Screenplay.Actors;
using Suzianna.Rest.Screenplay.Interactions;
using Suzianna.Rest.Screenplay.Questions;

namespace Neo.Specs.ScreenPlay.StreamEventTypes.Tasks;

public class ModifyStreamEventTypeByApiTask : ITask
{
    public ModifyStreamEventTypeByApiTask(ModifyStreamEventTypeCommand command)
    {
        _command = command;
    }

    private readonly ModifyStreamEventTypeCommand _command;

    public void PerformAs<T>(T actor) where T : Actor
    {
        while (true)
        {
            actor.AttemptsTo(Get.ResourceAt($"/api/StreamEventTypesQuery/{_command.Id}"));
            var response = actor.AsksFor(LastResponse.Content<StreamEventTypeResponse>());
            if (response.Status.Completed)
                break;
        }
        actor.AttemptsTo(Put.DataAsJson(_command)
            .To($"/api/StreamEventTypes/{_command.Id}"));
    }
}