using Suzianna.Core.Screenplay.Actors;
using Suzianna.Core.Screenplay.Questions;
using Suzianna.Rest.Screenplay.Interactions;
using Suzianna.Rest.Screenplay.Questions;

namespace Neo.Specs.ScreenPlay.StreamContexts.Questions;

public class GetStreamContextByIdQuestion : IQuestion<StreamContextResponse>
{
    private readonly Guid _id;

    public GetStreamContextByIdQuestion(Guid id)
    {
        _id = id;
    }

    public StreamContextResponse AnsweredBy(Actor actor)
    {
        actor.AttemptsTo(Get.ResourceAt($"/api/streamContextsQuery/{_id}"));
        var response = actor.AsksFor(LastResponse.Content<StreamContextResponse>());
        return response;
    }
}