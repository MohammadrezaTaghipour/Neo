using Neo.Specs.Framework;
using Neo.Specs.ScreenPlay.StreamEventTypes.Commands;
using TechTalk.SpecFlow;

namespace Neo.Specs.Features.StreamEventTypes.Given;

[Binding]
public class StreamEventTypeHasBeenRemoved
{
    private readonly ICommandBus _commandBus;
    private readonly ScenarioContext _context;

    public StreamEventTypeHasBeenRemoved(ScenarioContext context,
        ICommandBus commandBus)
    {
        _context = context;
        _commandBus = commandBus;
    }

    [Given("Stream event type '(.*)' has been removed")]
    public void Func(string title)
    {
        var command = new RemoveStreamEventTypeCommand
        {
            Id = _context.Get<Guid>(title),
            Version = 0
        };
        _commandBus.Dispatch(command);
    }
}