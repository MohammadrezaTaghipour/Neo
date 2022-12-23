using Neo.Specs.Framework;
using Neo.Specs.ScreenPlay.StreamEventTypes.Commands;
using Suzianna.Core.Screenplay;
using Suzianna.Core.Screenplay.Actors;
using TechTalk.SpecFlow;

namespace Neo.Specs.Features.StreamEventTypes.When;

[Binding]
public class IRemoveStreamEventType
{
    private readonly ICommandBus _commandBus;
    private readonly ScenarioContext _context;

    public IRemoveStreamEventType(ScenarioContext context,
        ICommandBus commandBus)
    {
        _context = context;
        _commandBus = commandBus;
    }

    [When("I remove stream event type '(.*)'")]
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