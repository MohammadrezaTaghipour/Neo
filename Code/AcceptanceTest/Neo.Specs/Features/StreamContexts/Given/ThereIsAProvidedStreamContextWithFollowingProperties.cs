using Neo.Specs.Framework;
using Neo.Specs.ScreenPlay.StreamContexts.Commands;
using Neo.Specs.ScreenPlay.StreamEventTypes.Commands;
using Suzianna.Core.Screenplay;
using Suzianna.Core.Screenplay.Actors;
using Suzianna.Rest.Screenplay.Questions;
using TechTalk.SpecFlow;

namespace Neo.Specs.Features.StreamContexts.Given;

[Binding]
public class ThereIsAProvidedStreamContextWithFollowingProperties
{
    private readonly ScenarioContext _context;
    private readonly ICommandBus _commandBus;
    private readonly Actor _actor;

    public ThereIsAProvidedStreamContextWithFollowingProperties(
        ScenarioContext context, ICommandBus commandBus,
        Stage stage)
    {
        _context = context;
        _commandBus = commandBus;
        _actor = stage.ActorInTheSpotlight;
    }


    [Given("there is a provided stream context with following properties")]
    public void Func(DefineStreamContextCommand command)
    {
        _context.Set(command);
    }

    [Given("With following stream event types")]
    public void Func(IReadOnlyCollection<StreamEventTypeCommandItem> items)
    {
        var command = _context.Get<DefineStreamContextCommand>();
        command.StreamEventTypes = items;
        _context.Set(command);
    }

    [Given("There is a defined stream context '(.*)'")]
    public void Func(string title)
    {
        var command = _context.Get<DefineStreamContextCommand>();
        _commandBus.Dispatch(command);
        command.Id = _actor.AsksFor(LastResponse.Content<Guid>());
        _context.Set(command.Id, title);
    }
}
