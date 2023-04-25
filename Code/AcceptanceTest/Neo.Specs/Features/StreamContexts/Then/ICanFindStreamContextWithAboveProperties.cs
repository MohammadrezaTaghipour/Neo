using FluentAssertions;
using Neo.Specs.ScreenPlay.StreamContexts.Commands;
using Neo.Specs.ScreenPlay.StreamContexts.Questions;
using Suzianna.Core.Screenplay;
using Suzianna.Core.Screenplay.Actors;
using TechTalk.SpecFlow;

namespace Neo.Specs.Features.StreamContexts.Then;

[Binding]
public class ICanFindStreamContextWithAboveProperties
{
    private readonly ScenarioContext _context;
    private readonly Actor _actor;

    public ICanFindStreamContextWithAboveProperties(
        ScenarioContext context, Stage stage)
    {
        _context = context;
        _actor = stage.ActorInTheSpotlight;
    }

    [Then("I can find stream context '(.*)' with above properties")]
    public void Func(string title)
    {
        if (_context.Keys.Any(k => k.Contains(nameof(ModifyStreamContextCommand))))
            AssertModification(title);
        else
            AssertDefinition(title);
    }

    private void AssertDefinition(string expectedTitle)
    {
        var expected = _context.Get<DefineStreamContextCommand>();
        var actual = _actor.AsksFor(new GetStreamContextByIdQuestion(expected.Id));

        actual.Should().BeEquivalentTo(expected, opt => opt
            .Excluding(_ => _.StreamEventTypes));

        if (expected.StreamEventTypes.Any())
            actual.StreamEventTypes.OrderBy(streamEventTypeId => streamEventTypeId)
                .Should()
                .BeEquivalentTo(expected.StreamEventTypes.Select(_ => _.StreamEventTypeId)
                .OrderBy(streamEventTypeId => streamEventTypeId));
    }

    private void AssertModification(string expectedTitle)
    {
        var expected = _context.Get<ModifyStreamContextCommand>();
        var actual = _actor.AsksFor(new GetStreamContextByIdQuestion(expected.Id));

        actual.Should().BeEquivalentTo(expected, opt => opt
            .Excluding(_ => _.StreamEventTypes)
            .Excluding(_ => _.Version));

        if (expected.StreamEventTypes.Any())
            actual.StreamEventTypes.OrderBy(streamEventTypeId => streamEventTypeId)
                .Should()
                .BeEquivalentTo(expected.StreamEventTypes.Select(_ => _.StreamEventTypeId)
                .OrderBy(streamEventTypeId => streamEventTypeId));
    }
}
