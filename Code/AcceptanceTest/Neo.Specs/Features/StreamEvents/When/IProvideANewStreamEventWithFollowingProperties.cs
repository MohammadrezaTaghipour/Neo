using Neo.Specs.Framework;
using Neo.Specs.ScreenPlay.StreamEvents.Commands;
using TechTalk.SpecFlow;

namespace Neo.Specs.Features.StreamEvents.When;

[Binding]
public class IProvideANewStreamEventWithFollowingProperties
{
    private readonly ScenarioContext _context;
    private readonly ICommandBus _commandBus;

    public IProvideANewStreamEventWithFollowingProperties(
        ScenarioContext context, ICommandBus commandBus)
    {
        _context = context;
        _commandBus = commandBus;
    }

    [When("I provide a new stream event with following properties")]
    public void Func(AppendStreamEventCommand command)
    {
        _context.Set(command);
    }

    [When("With the following stream event metadata")]
    public void Func(IReadOnlyCollection<StreamEventMetadaCommandItem> commandItems)
    {
        var command = _context.Get<AppendStreamEventCommand>();
        command.Metadata = commandItems;
    }

    [When("I append stream event '(.*)'")]
    public void Func(string title)
    {
        var command = _context.Get<AppendStreamEventCommand>();
        _commandBus.Dispatch(command);
    }
}
