using Neo.Specs.Framework;
using Neo.Specs.ScreenPlay.StreamContexts.Commands;
using TechTalk.SpecFlow;

namespace Neo.Specs.Features.StreamContexts.When;

[Binding]
public class IProvideANewStreamContextWithFollowingProperties
{
    private readonly ScenarioContext _context;
    private readonly ICommandBus _commandBus;

    public IProvideANewStreamContextWithFollowingProperties(
        ScenarioContext context, ICommandBus commandBus)
    {
        _context = context;
        _commandBus = commandBus;
    }


    [When("I provide a new stream context with following properties")]
    public void Func(DefineStreamContextCommand command)
    {
        _context.Set(command);
    }

    [When("With following stream event type")]
    public void Func(IReadOnlyCollection<StreamEventTypeCommandItem> items)
    {
        var command = _context.Get<DefineStreamContextCommand>();
        command.StreamEventTypes = items;
        _context.Set(command);
    }

    [When("I define stream context '(.*)'")]
    public void Func(string title)
    {
        var command = _context.Get<DefineStreamContextCommand>();
        _commandBus.Dispatch(command);
    }
}
