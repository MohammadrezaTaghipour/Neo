using Neo.Specs.Framework;
using Neo.Specs.ScreenPlay.StreamEventTypes.Commands;
using TechTalk.SpecFlow;

namespace Neo.Specs.Features.StreamEventTypes.When;

[Binding]
public class IDefineANewStreamEventTypeWithFollowingProperties
{
    private readonly ScenarioContext _context;
    private readonly ICommandBus _commandBus;

    public IDefineANewStreamEventTypeWithFollowingProperties(
        ScenarioContext context, ICommandBus commandBus)
    {
        _context = context;
        _commandBus = commandBus;
    }

    [When("I define a new stream event type with following properties")]
    public void Func(DefineStreamEventTypeCommand command)
    {
        _commandBus.Dispatch(command);
        _context.Set(command);
    }
}
