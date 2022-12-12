using Neo.Specs.Framework;
using Neo.Specs.ScreenPlay.LifeStreams.Commands;
using TechTalk.SpecFlow;

namespace Neo.Specs.Features.LifeStreams.When;

[Binding]
public class IModifyLifeStreamWithFollwoingProperties
{
    private readonly ICommandBus _commandBus;
    private readonly ScenarioContext _context;

    public IModifyLifeStreamWithFollwoingProperties(
        ICommandBus commandBus, ScenarioContext context)
    {
        _commandBus = commandBus;
        _context = context;
    }

    [When("I modify life stream '(.*)' with follwoing properties")]
    public void Func(string title, ModifyLifeStreamCommand command)
    {
        _commandBus.Dispatch(command);
        _context.Set(command.Id, command.Title);
        _context.Set(command);
    }
}