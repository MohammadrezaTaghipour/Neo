using Neo.Specs.Framework;
using Neo.Specs.ScreenPlay.StreamContexts.Commands;
using TechTalk.SpecFlow;

namespace Neo.Specs.Features.StreamContexts.When;

[Binding]
public class IReprovideStreamContextWithFollowingProperties
{
    private readonly ScenarioContext _context;
    private readonly ICommandBus _commandBus;

    public IReprovideStreamContextWithFollowingProperties(
        ScenarioContext context, ICommandBus commandBus)
    {
        _context = context;
        _commandBus = commandBus;
    }

    [When("I reprovide stream context 'Career path' with following properties")]
    public void Func(ModifyStreamContextCommand command)
    {
        _context.Set(command);
    }

    [When("I reprovide the following stream event types")]
    public void Func(IReadOnlyCollection<StreamEventTypeCommandItem> items)
    {
        var command = _context.Get<ModifyStreamContextCommand>();
        command.StreamEventTypes = items;
        _context.Set(command);
    }

    [When("I modify stream context '(.*)'")]
    public void Func(string title)
    {
        var command = _context.Get<ModifyStreamContextCommand>();
        _commandBus.Dispatch(command);
    }
}
