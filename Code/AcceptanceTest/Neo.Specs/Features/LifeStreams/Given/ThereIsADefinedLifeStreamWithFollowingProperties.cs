using Neo.Specs.Framework;
using Neo.Specs.ScreenPlay.LifeStreams.Commands;
using Neo.Specs.ScreenPlay.StreamContexts.Commands;
using Suzianna.Core.Screenplay;
using Suzianna.Core.Screenplay.Actors;
using Suzianna.Rest.Screenplay.Questions;
using TechTalk.SpecFlow;

namespace Neo.Specs.Features.LifeStreams.Given;

[Binding]
public class ThereIsADefinedLifeStreamWithFollowingProperties
{
    private readonly ICommandBus _commandBus;
    private readonly ScenarioContext _context;
    private readonly Actor _actor;

    public ThereIsADefinedLifeStreamWithFollowingProperties(
        ICommandBus commandBus, ScenarioContext context,
        Stage stage)
    {
        _commandBus = commandBus;
        _context = context;
        _actor = stage.ActorInTheSpotlight;
    }

    [Given("There is a defined life stream with following properties")]
    public void Func(DefineLifeStreamCommand command)
    {
        _commandBus.Dispatch(command);
        command.Id = _actor.AsksFor(LastResponse.Content<Guid>());
        _context.Set(command.Id, command.Title);
        _context.Set(command);
    }
}
