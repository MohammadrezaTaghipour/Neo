using FluentAssertions;
using Neo.Specs.ScreenPlay.LifeStreams.Questions;
using Suzianna.Core.Screenplay;
using Suzianna.Core.Screenplay.Actors;
using TechTalk.SpecFlow;

namespace Neo.Specs.Features.LifeStreams.Then;

[Binding]
public class ICanNotFindLifeStreamWithAboveProperties
{
    private readonly ScenarioContext _context;
    private readonly Actor _actor;

    public ICanNotFindLifeStreamWithAboveProperties(
        ScenarioContext context, Stage stage)
    {
        _context = context;
        _actor = stage.ActorInTheSpotlight;
    }

    [Then("I can not find life stream '(.*)' with above properties")]
    public void Func(string title)
    {
        var expectedId = _context.Get<Guid>(title);
        var actual = _actor.AsksFor(new GetLifeStreamByIdQuestion(expectedId));

        actual.Removed.Should().BeTrue();
    }
}
