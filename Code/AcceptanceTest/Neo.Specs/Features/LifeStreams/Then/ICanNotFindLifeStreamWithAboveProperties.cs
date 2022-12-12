using Suzianna.Core.Screenplay.Actors;
using Suzianna.Core.Screenplay;
using TechTalk.SpecFlow;
using Neo.Specs.ScreenPlay.LifeStreams.Questions;
using FluentAssertions;

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

        actual.Deleted.Should().BeTrue();
    }
}
