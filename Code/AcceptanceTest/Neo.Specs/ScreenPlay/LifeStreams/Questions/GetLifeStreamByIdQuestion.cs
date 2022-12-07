using Suzianna.Core.Screenplay.Actors;
using Suzianna.Core.Screenplay.Questions;
using Suzianna.Rest.Screenplay.Interactions;
using Suzianna.Rest.Screenplay.Questions;

namespace Neo.Specs.ScreenPlay.LifeStreams.Questions;

public class GetLifeStreamByIdQuestion : IQuestion<LifeStreamResponse>
{
    private readonly Guid _id;

    public GetLifeStreamByIdQuestion(Guid id)
    {
        _id = id;
    }

    public LifeStreamResponse AnsweredBy(Actor actor)
    {
        actor.AttemptsTo(Get.ResourceAt($"/api/LifeStreamsQuery/{_id}"));
        return actor.AsksFor(LastResponse.Content<LifeStreamResponse>());
    }
}