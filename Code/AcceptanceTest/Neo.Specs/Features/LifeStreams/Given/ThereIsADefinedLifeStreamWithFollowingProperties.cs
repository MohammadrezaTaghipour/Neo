using Neo.Specs.Framework;
using Neo.Specs.ScreenPlay.LifeStreams.Commands;
using TechTalk.SpecFlow;

namespace Neo.Specs.Features.LifeStreams.Given;

[Binding]
public class ThereIsADefinedLifeStreamWithFollowingProperties
{
    private readonly ICommandBus _commandBus;
    private readonly ScenarioContext _context;

    public ThereIsADefinedLifeStreamWithFollowingProperties(
        ICommandBus commandBus, ScenarioContext context)
    {
        _commandBus = commandBus;
        _context = context;
    }

    [Given("There is a defined life stream with following properties")]
    public void Func(DefineLifeStreamCommand command)
    {
        _commandBus.Dispatch(command);
        _context.Set(command.Id, command.Title);
        _context.Set(command);
    }
}
