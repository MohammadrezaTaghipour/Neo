using FluentAssertions;
using Neo.Specs.Framework;
using Neo.Specs.ScreenPlay.LifeStreams.Questions;
using Neo.Specs.ScreenPlay.StreamEvents.Commands;
using Suzianna.Core.Screenplay;
using Suzianna.Core.Screenplay.Actors;
using TechTalk.SpecFlow;

namespace Neo.Specs.Features.StreamEvents.Then;

[Binding]
public class ICanNotFindStreamEventWithAboveProperties
{
    private readonly ScenarioContext _context;
    private readonly ICommandBus _commandBus;
    private readonly Actor _actor;

    public ICanNotFindStreamEventWithAboveProperties(
        ScenarioContext context, ICommandBus commandBus,
        Stage stage)
    {
        _context = context;
        _commandBus = commandBus;
        _actor = stage.ActorInTheSpotlight;
    }

    [Then("I can not find stream event '(.*)' with above properties")]
    public void Func(string title)
    {
        var lifeStreamId = _context.Get<AppendStreamEventCommand>(title).LifeStreamId;
        var expectedId = _context.Get<RemoveStreamEventCommand>().Id;
        var actual = _actor.AsksFor(new GetLifeStreamByIdQuestion(lifeStreamId))
            .StreamEvents;

        actual.Where(_ => _.Id == expectedId).Should().BeNullOrEmpty();
    }
}
