using MassTransit;
using Neo.Application.Contracts.ReferentialPointers;

namespace Neo.Application.StreamContexts;

public class StreamContextMachineState :
    SagaStateMachineInstance,
    ISagaVersion
{
    public StreamContextMachineState()
    {
        ReferentialPointerCurrentState = new();
        ReferentialPointerNextState = new();
    }

    public Guid CorrelationId { get; set; }
    public int Version { get; set; }
    public string CurrentState { get; set; }

    public Guid StreamContextId { get; set; }
    public ReferentialPointerContainer ReferentialPointerCurrentState { get; set; }
    public ReferentialPointerContainer ReferentialPointerNextState { get; set; }
}