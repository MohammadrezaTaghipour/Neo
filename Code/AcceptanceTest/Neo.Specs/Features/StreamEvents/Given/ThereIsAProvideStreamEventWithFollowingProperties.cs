using Neo.Specs.Framework;
using Neo.Specs.ScreenPlay.StreamEvents.Commands;
using TechTalk.SpecFlow;

namespace Neo.Specs.Features.StreamEvents.Given;

[Binding]
public class ThereIsAProvideStreamEventWithFollowingProperties
{
    private readonly ScenarioContext _context;
    private readonly ICommandBus _commandBus;

    public ThereIsAProvideStreamEventWithFollowingProperties(
        ScenarioContext context, ICommandBus commandBus)
    {
        _context = context;
        _commandBus = commandBus;
    }

    [Given("There is a provide stream event '(.*)' with following properties")]
    public void Func(string title, AppendStreamEventCommand command)
    {
        _context.Set(command, title);
    }

    [Given("With the following stream event metadata of stream event '(.*)'")]
    public void Func(string title, IReadOnlyCollection<StreamEventMetadaCommandItem> commandItems)
    {
        var command = _context.Get<AppendStreamEventCommand>(title);
        command.Metadata = commandItems;
    }

    [Given("There is a appended stream event '(.*)'")]
    public void Func(string title)
    {
        var command = _context.Get<AppendStreamEventCommand>(title);
        _commandBus.Dispatch(command);
    }
}
