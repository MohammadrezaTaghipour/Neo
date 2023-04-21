using MassTransit.SagaStateMachine;
using MassTransit.Visualizer;
using Neo.Application.LifeStreams;
using Xunit;

namespace Neo.Domain.Tests.Unit.LifeStreams
{
    public class LifeStreamStateMachineTest
    {
        [Fact]
        public void Show_me_the_state_machine()
        {
            var Machine = new LifeStreamStateMachine();

            var graph = Machine.GetGraph();

            var generator = new StateMachineGraphvizGenerator(graph);

            string dots = generator.CreateDotFile();

            Console.WriteLine(dots);
        }

    }
}
