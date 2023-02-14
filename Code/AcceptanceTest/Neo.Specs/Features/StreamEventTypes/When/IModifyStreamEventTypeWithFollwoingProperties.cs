using Neo.Specs.Framework;
using Neo.Specs.ScreenPlay.StreamEventTypes.Commands;
using TechTalk.SpecFlow;

namespace Neo.Specs.Features.StreamEventTypes.When
{
    [Binding]
    public class IModifyStreamEventTypeWithFollwoingProperties
    {
        private readonly ScenarioContext _context;
        private readonly ICommandBus _commandBus;

        public IModifyStreamEventTypeWithFollwoingProperties(
            ScenarioContext context, ICommandBus commandBus)
        {
            _context = context;
            _commandBus = commandBus;
        }

        [When("I modify stream event type '(.*)' with follwoing properties")]
        public void Func(string title, ModifyStreamEventTypeCommand command)
        {
            _commandBus.Dispatch(command);
            _context.Set(command);
            _context.Set(command.Id, command.Title);
        }
    }
}
