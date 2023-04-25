using Suzianna.Core.Screenplay.Actors;
using Suzianna.Core.Screenplay.Questions;
using Suzianna.Rest.Screenplay.Interactions;
using Suzianna.Rest.Screenplay.Questions;

namespace Neo.Specs.ScreenPlay.Notifications.Questions;

public class GetRequestStatusResponse : IQuestion<RequestStatusResponse>
{
    private readonly string _id;

    public GetRequestStatusResponse(string id)
    {
        _id = id;
    }

    public RequestStatusResponse AnsweredBy(Actor actor)
    {
        while (true)
        {
            actor.AttemptsTo(Get.ResourceAt($"/api/notificationsQuery/status/{_id}"));
            var response = actor.AsksFor(LastResponse.Content<RequestStatusResponse>());
            if (response != null)
                if (response.Completed)
                {
                    if (response.Faulted)
                    {
                        actor.Recall<LastRequestResponse>().Set(
                            response.ErrorCode,
                            response.ErrorMessage);
                    }
                    return response;
                }
        }
    }
}
