using Suzianna.Core.Screenplay.Actors;
using Suzianna.Core.Screenplay.Questions;
using Suzianna.Rest.Screenplay.Interactions;
using Suzianna.Rest.Screenplay.Questions;

namespace Neo.Specs.ScreenPlay.StreamEventTypes.Questions;

public class GetStreamEventTypeByIdQuestion : IQuestion<StreamEventTypeResponse>
{
    private readonly Guid _id;

    public GetStreamEventTypeByIdQuestion(Guid id)
    {
        _id = id;
    }

    public StreamEventTypeResponse AnsweredBy(Actor actor)
    {
        actor.AttemptsTo(Get.ResourceAt($"/api/StreamEventTypesQuery/{_id}"));
        return actor.AsksFor(LastResponse.Content<StreamEventTypeResponse>());
    }
}
