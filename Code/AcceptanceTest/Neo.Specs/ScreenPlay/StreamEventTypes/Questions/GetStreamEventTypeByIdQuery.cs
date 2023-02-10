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
        while (true)
        {
            actor.AttemptsTo(Get.ResourceAt($"/api/StreamEventTypesQuery/{_id}"));
            var response = actor.AsksFor(LastResponse.Content<StreamEventTypeResponse>());
            if (response != null && response.Status != null)
                if (response.Status.Completed && response.Status.Faulted)
                {
                    LastResponseException.Set(response.Status.ErrorCode,
                        response.Status.ErrorMessage);
                    return null;
                }
                else if(response.Status.Completed)
                    return response;
        }
    }
}
