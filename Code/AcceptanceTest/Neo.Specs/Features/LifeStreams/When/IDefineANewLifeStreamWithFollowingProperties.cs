using Neo.Specs.Framework;
using Neo.Specs.ScreenPlay.LifeStreams.Commands;
using TechTalk.SpecFlow;

namespace Neo.Specs.Features.LifeStreams.When
{
    [Binding]
    public class IDefineANewLifeStreamWithFollowingProperties
    {
        private readonly ICommandBus _commandBus;
        private readonly ScenarioContext _context;

        public IDefineANewLifeStreamWithFollowingProperties(
            ICommandBus commandBus, ScenarioContext context)
        {
            _commandBus = commandBus;
            _context = context;
        }

        [When("I define a new life stream with following properties")]
        public void Func(DefineLifeStreamCommand command)
        {
            _commandBus.Dispatch(command);
            _context.Set(command);
        }
    }
}
