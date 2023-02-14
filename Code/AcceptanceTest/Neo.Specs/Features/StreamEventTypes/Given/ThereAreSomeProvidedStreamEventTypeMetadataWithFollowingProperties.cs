using Neo.Specs.ScreenPlay.StreamEventTypes.Commands;
using TechTalk.SpecFlow;

namespace Neo.Specs.Features.StreamEventTypes.Given;

[Binding]
public class ThereAreSomeProvidedStreamEventTypeMetadataWithFollowingProperties
{
    private readonly ScenarioContext _context;

    public ThereAreSomeProvidedStreamEventTypeMetadataWithFollowingProperties
        (ScenarioContext context)
    {
        _context = context;
    }

    [Given("There are some provided stream event type metadata with following properties")]
    [Given("I have reprovided some stream event type metadata with following properties")]

    public void Func(
        IReadOnlyCollection<StreamEventTypeMetadataCommandItem> commandItems)
    {
        _context.Set(commandItems);
    }
}
