using Suzianna.Core.Screenplay.Actors;
using Suzianna.Core.Screenplay;
using TechTalk.SpecFlow;
using FluentAssertions;
using Neo.Specs.ScreenPlay.LifeStreams.Questions;
using Neo.Specs.ScreenPlay.StreamEvents.Commands;

namespace Neo.Specs.Features.StreamEvents.Then;

[Binding]
public class ICanFindStreamEventWithAboveProperties
{
    private readonly ScenarioContext _context;
    private readonly Actor _actor;
    public ICanFindStreamEventWithAboveProperties(
        ScenarioContext context, Stage stage)
    {
        _context = context;
        _actor = stage.ActorInTheSpotlight;
    }

    [Then("I can find stream event '(.*)' with above properties")]
    public void Func(string title)
    {
        var expected = _context.Get<AppendStreamEventCommand>();
        var actual = _actor.AsksFor(new GetLifeStreamByIdQuestion(expected.LifeStreamId));

        actual.StreamEvents.Should().ContainEquivalentOf(expected, opt => opt
            .Excluding(_ => _.Id)
            .Excluding(_ => _.OperationType));
    }
}
