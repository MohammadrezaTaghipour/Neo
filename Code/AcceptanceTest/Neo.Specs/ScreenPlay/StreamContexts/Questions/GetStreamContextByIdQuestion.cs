using Neo.Specs.ScreenPlay.StreamEventTypes.Questions;
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
        while (true)
        {
            actor.AttemptsTo(Get.ResourceAt($"/api/StreamContextsQuery/{_id}"));
            var response = actor.AsksFor(LastResponse.Content<StreamContextResponse>());
            if (response != null && response.Status != null)
                if (response.Status.Completed)
                {
                    if (response.Status.Faulted)
                    {
                        LastResponseException.Set(  
                            response.Status.ErrorCode,
                            response.Status.ErrorMessage);
                    }
                    return response;
                }
        }
    }
}