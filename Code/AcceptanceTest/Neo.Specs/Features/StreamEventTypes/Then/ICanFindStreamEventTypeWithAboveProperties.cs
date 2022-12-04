using FluentAssertions;
using Neo.Specs.ScreenPlay.StreamEventTypes.Commands;
using Neo.Specs.ScreenPlay.StreamEventTypes.Questions;
using Suzianna.Core.Screenplay;
using Suzianna.Core.Screenplay.Actors;
using Suzianna.Rest.Screenplay.Questions;
using TechTalk.SpecFlow;

namespace Neo.Specs.Features.StreamEventTypes.Then;

[Binding]
public class ICanFindStreamEventTypeWithAboveProperties
{
    private readonly ScenarioContext _context;
    private readonly Actor _actor;

    public ICanFindStreamEventTypeWithAboveProperties(
        ScenarioContext context, Stage stage)
    {
        _context = context;
        _actor = stage.ActorInTheSpotlight;
    }

    [Then("I can find stream event type '(.*)' with above properties")]
    public void Func(string title)
    {
        if (_context.Keys.Any(k => k.Contains(nameof(ModifyStreamEventTypeCommand))))
            AssertModification(title);
        else
            AssertDefinition(title);
    }

    private void AssertDefinition(string expectedTitle)
    {
        var expected = _context.Get<DefineStreamEventTypeCommand>();
        expected.Id = _actor.AsksFor(LastResponse.Content<Guid>());
        var actual = _actor.AsksFor(new GetStreamEventTypeByIdQuestion(expected.Id));

        actual.Should().BeEquivalentTo(expected, opt => opt
            .Excluding(_ => _.Id)
            .Excluding(_ => _.Metadata));

        if (expected.Metadata.Any())
            actual.Metadata.OrderBy(_ => _.Title)
                .Should()
                .BeEquivalentTo(expected.Metadata.OrderBy(_ => _.Title));
    }

    private void AssertModification(string expectedTitle)
    {
        var id = _context.Get<Guid>(expectedTitle);
        var expected = _context.Get<ModifyStreamEventTypeCommand>();
        var actual = _actor.AsksFor(new GetStreamEventTypeByIdQuestion(expected.Id));

        actual.Should().BeEquivalentTo(expected, opt => opt
            .Excluding(_ => _.Id)
            .Excluding(_ => _.Metadata)
            .Excluding(_ => _.Version));

        if (expected.Metadata.Any())
            actual.Metadata.OrderBy(_ => _.Title)
                .Should()
                .BeEquivalentTo(expected.Metadata.OrderBy(_ => _.Title));
    }
}
