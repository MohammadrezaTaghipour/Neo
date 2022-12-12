using Neo.Specs.Framework;
using Neo.Specs.ScreenPlay.LifeStreams.Commands;
using TechTalk.SpecFlow;

namespace Neo.Specs.Features.LifeStreams.When;

[Binding]
public class IRemoveLifeStream
{
    private readonly ICommandBus _commandBus;
    private readonly ScenarioContext _context;

    public IRemoveLifeStream(ScenarioContext context,
        ICommandBus commandBus)
    {
        _context = context;
        _commandBus = commandBus;
    }

    [When("I remove life stream '(.*)'")]
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