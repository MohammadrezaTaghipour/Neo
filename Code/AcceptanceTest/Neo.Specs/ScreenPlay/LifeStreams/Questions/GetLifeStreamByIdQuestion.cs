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
        while (true)
        {
            actor.AttemptsTo(Get.ResourceAt($"/api/LifeStreamsQuery/{_id}"));
            var response = actor.AsksFor(LastResponse.Content<LifeStreamResponse>());
            if (response != null && response.Status != null)
                if (response.Status.Completed)
                {
                    if (response.Status.Faulted)
                    {
                        actor.Recall<LastRequestResponse>().Set(
                            response.Status.ErrorCode,
                            response.Status.ErrorMessage);
                    }
                    return response;
                }
        }
    }
}
