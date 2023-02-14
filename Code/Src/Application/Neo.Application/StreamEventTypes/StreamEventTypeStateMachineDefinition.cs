using MassTransit;

namespace Neo.Application.StreamEventTypes;

public class StreamEventTypeStateMachineDefinition :
        SagaDefinition<StreamEventTypeMachineState>
{
    public StreamEventTypeStateMachineDefinition()
    {
        ConcurrentMessageLimit = 1;
    }

    protected override void ConfigureSaga(
        IReceiveEndpointConfigurator endpointConfigurator, 
        ISagaConfigurator<StreamEventTypeMachineState> sagaConfigurator)
    {
        endpointConfigurator.UseMessageRetry(r => r.Intervals(2, 3000));
        endpointConfigurator.UseInMemoryOutbox();
    }
}