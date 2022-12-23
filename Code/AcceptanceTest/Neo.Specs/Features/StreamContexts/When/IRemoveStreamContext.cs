using Neo.Specs.Framework;
using Neo.Specs.ScreenPlay.StreamContexts.Commands;
using TechTalk.SpecFlow;

namespace Neo.Specs.Features.StreamContexts.When;

[Binding]
public class IRemoveStreamContext
{
    private readonly ICommandBus _commandBus;
    private readonly ScenarioContext _context;

    public IRemoveStreamContext(ScenarioContext context,
        ICommandBus commandBus)
    {
        _context = context;
        _commandBus = commandBus;
    }

    [When("I remove stream context '(.*)'")]
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
