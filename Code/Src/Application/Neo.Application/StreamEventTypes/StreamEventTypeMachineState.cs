using MassTransit;
using Neo.Application.Contracts.ReferentialPointers;

namespace Neo.Application.StreamEventTypes;

public class StreamEventTypeMachineState :
    SagaStateMachineInstance,
    ISagaVersion
{
    public StreamEventTypeMachineState()
    {
        ReferentialPointerCurrentState = new();
        ReferentialPointerNextState = new();
    }

    public Guid CorrelationId { get; set; }
    public int Version { get; set; }
    public string CurrentState { get; set; }

    public Guid StreamEventTypeId { get; set; }
    public string ErrorCode { get; set; }
    public string ErrorMessage { get; set; }

    public ReferentialPointerContainer ReferentialPointerCurrentState { get; set; }
    public ReferentialPointerContainer ReferentialPointerNextState { get; set; }
}
