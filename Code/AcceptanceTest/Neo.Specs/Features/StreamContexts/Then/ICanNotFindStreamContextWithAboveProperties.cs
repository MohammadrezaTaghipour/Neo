using Suzianna.Core.Screenplay.Actors;
using Suzianna.Core.Screenplay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Neo.Specs.ScreenPlay.LifeStreams.Questions;
using Neo.Specs.ScreenPlay.StreamContexts.Questions;
using FluentAssertions;

namespace Neo.Specs.Features.StreamContexts.Then;

[Binding]
public class ICanNotFindStreamContextWithAboveProperties
{
    private readonly ScenarioContext _context;
    private readonly Actor _actor;

    public ICanNotFindStreamContextWithAboveProperties(
        ScenarioContext context, Stage stage)
    {
        _context = context;
        _actor = stage.ActorInTheSpotlight;
    }

    [Then("I can not find stream context '(.*)' with above properties")]
    public void Func(string title)
    {
        var expectedId = _context.Get<Guid>(title);
        var actual = _actor.AsksFor(new GetStreamContextByIdQuestion(expectedId));

        actual.Removed.Should().BeTrue();
    }
}