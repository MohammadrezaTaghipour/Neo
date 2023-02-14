using FluentAssertions;
using Neo.Specs.ScreenPlay.StreamEventTypes.Questions;
using Suzianna.Core.Screenplay;
using Suzianna.Core.Screenplay.Actors;
using TechTalk.SpecFlow;

namespace Neo.Specs.Features.StreamEventTypes.Then;

[Binding]
public class ICanNotFindStreamEventTypeWithAboveProperties
{
    private readonly ScenarioContext _context;
    private readonly Actor _actor;

    public ICanNotFindStreamEventTypeWithAboveProperties(
        ScenarioContext context, Stage stage)
    {
        _context = context;
        _actor = stage.ActorInTheSpotlight;
    }

    [Then("I can not find stream event type '(.*)' with above properties")]
    public void Func(string title)
    {
        var expectedId = _context.Get<Guid>(title);
        var actual = _actor.AsksFor(new GetStreamEventTypeByIdQuestion(expectedId));

        actual.Removed.Should().BeTrue();
    }
}