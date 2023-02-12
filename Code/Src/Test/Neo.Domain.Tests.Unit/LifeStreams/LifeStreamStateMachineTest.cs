using MassTransit.SagaStateMachine;
using MassTransit.Visualizer;
using Neo.Application.LifeStreams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
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
