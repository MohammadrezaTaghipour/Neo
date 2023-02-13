using Neo.Specs.Framework;
using Neo.Specs.ScreenPlay.StreamEventTypes.Commands;
using Suzianna.Core.Screenplay;
using TechTalk.SpecFlow;

namespace Neo.Specs.Features.StreamEventTypes.Given;

[Binding]
public class ThereIsADefinedStreamEventTypeWithFollowingProperties
{
    private readonly ScenarioContext _context;
    private readonly ICommandBus _commandBus;

    public ThereIsADefinedStreamEventTypeWithFollowingProperties(
        ScenarioContext context, ICommandBus commandBus, Stage stage)
    {
        _context = context;
        _commandBus = commandBus;
    }

    [Given("There is a defined stream event type with following properties")]
    public void Func(DefineStreamEventTypeCommand command)
    {
        _commandBus.Dispatch(command);
        _context.Set(command);
        _context.Set(command.Id, command.Title);
    }

    [Given("There are some defined stream event types with following properties")]
    public void Func(IReadOnlyCollection<DefineStreamEventTypeCommand> commands)
    {
        foreach (var command in commands)
        {
            _commandBus.Dispatch(command);
            _context.Set(command);
            _context.Set(command.Id, command.Title);
        }
    }
}
