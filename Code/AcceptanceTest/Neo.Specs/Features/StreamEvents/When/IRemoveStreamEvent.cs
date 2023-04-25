using Neo.Specs.Framework;
using Neo.Specs.ScreenPlay.LifeStreams.Questions;
using Neo.Specs.ScreenPlay.StreamEvents.Commands;
using Suzianna.Core.Screenplay;
using Suzianna.Core.Screenplay.Actors;
using TechTalk.SpecFlow;

namespace Neo.Specs.Features.StreamEvents.When;

[Binding]
public class IRemoveStreamEvent
{
    private readonly ScenarioContext _context;
    private readonly ICommandBus _commandBus;
    private readonly Actor _actor;

    public IRemoveStreamEvent(
        ScenarioContext context, ICommandBus commandBus,
        Stage stage)
    {
        _context = context;
        _commandBus = commandBus;
        _actor = stage.ActorInTheSpotlight;
    }

    [When("I remove stream event '(.*)'")]
    public void Func(string title)
    {
        var lifeStreamId = _context.Get<AppendStreamEventCommand>(title).LifeStreamId;
        var streamEvent = _actor.AsksFor(new GetLifeStreamByIdQuestion(lifeStreamId))
            .StreamEvents.Last();
        var command = new RemoveStreamEventCommand
        {
            Id = streamEvent.Id,
            LifeStreamId = lifeStreamId
        };
        _context.Set(command);
        _commandBus.Dispatch(command);
    }
}
