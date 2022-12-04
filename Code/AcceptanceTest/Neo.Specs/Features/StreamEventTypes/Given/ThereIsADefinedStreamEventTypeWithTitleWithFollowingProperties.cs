using Neo.Specs.Framework;
using Neo.Specs.ScreenPlay.StreamEventTypes.Commands;
using Suzianna.Core.Screenplay;
using Suzianna.Core.Screenplay.Actors;
using Suzianna.Rest.Screenplay.Questions;
using TechTalk.SpecFlow;

namespace Neo.Specs.Features.StreamEventTypes.Given;

[Binding]
public class ThereIsADefinedStreamEventTypeWithTitleWithFollowingProperties
{
    private readonly ScenarioContext _context;
    private readonly ICommandBus _commandBus;
    private readonly Actor _actor;

    public ThereIsADefinedStreamEventTypeWithTitleWithFollowingProperties(
        ScenarioContext context, ICommandBus commandBus, Stage stage)
    {
        _context = context;
        _commandBus = commandBus;
        _actor = stage.ActorInTheSpotlight;
    }

    [Given("There is a defined stream event type with following properties")]
    public void Func(DefineStreamEventTypeCommand command)
    {
        _commandBus.Dispatch(command);
        command.Id = _actor.AsksFor(LastResponse.Content<Guid>());
        _context.Set(command);
        _context.Set(command.Id, command.Title);
    }
}