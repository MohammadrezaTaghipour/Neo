using Neo.Specs.Framework;
using Neo.Specs.ScreenPlay.StreamContexts.Commands;
using TechTalk.SpecFlow;

namespace Neo.Specs.Features.StreamContexts.Given;

[Binding]
public class StreamContextHasBeenRemoved
{
    private readonly ICommandBus _commandBus;
    private readonly ScenarioContext _context;

    public StreamContextHasBeenRemoved(ScenarioContext context,
        ICommandBus commandBus)
    {
        _context = context;
        _commandBus = commandBus;
    }

    [Given("Stream context '(.*)' has been removed")]
    public void Func(string title)
    {
        var command = new RemoveStreamContextCommand
        {
            Id = _context.Get<Guid>(title),
            Version = 0
        };
        _commandBus.Dispatch(command);
    }
}