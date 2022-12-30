using Neo.Specs.Framework;
using Neo.Specs.ScreenPlay.LifeStreams.Commands;
using TechTalk.SpecFlow;

namespace Neo.Specs.Features.LifeStreams.Given;

[Binding]
public class LifeStreamHasBeenRemoved
{
    private readonly ICommandBus _commandBus;
    private readonly ScenarioContext _context;

    public LifeStreamHasBeenRemoved(ScenarioContext context,
        ICommandBus commandBus)
    {
        _context = context;
        _commandBus = commandBus;
    }

    [Given("Life stream '(.*)' has been removed")]
    public void Func(string title)
    {
        var command = new RemoveLifeStreamCommand
        {
            Id = _context.Get<Guid>(title),
            Version = 0
        };
        _commandBus.Dispatch(command);
    }
}