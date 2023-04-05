using Suzianna.Core.Screenplay.Actors;
using Suzianna.Core.Screenplay.Questions;

namespace Neo.Specs.ScreenPlay.StreamEventTypes.Questions;

public class GetAllStreamEventTypesQuestion :
    IQuestion<IReadOnlyCollection<StreamEventTypeResponse>>
{
    public IReadOnlyCollection<StreamEventTypeResponse> AnsweredBy(Actor actor)
    {
        throw new NotImplementedException();
    }
}
