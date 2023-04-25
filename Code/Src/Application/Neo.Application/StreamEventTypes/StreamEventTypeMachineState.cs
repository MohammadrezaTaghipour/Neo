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
        ProjectionSyncPosition = new();
    }

    public Guid CorrelationId { get; set; }
    public int Version { get; set; }
    public string CurrentState { get; set; }

    public Guid StreamEventTypeId { get; set; }
    public ReferentialPointerContainer ReferentialPointerCurrentState { get; set; }
    public ReferentialPointerContainer ReferentialPointerNextState { get; set; }
    public ProjectionSyncPosition ProjectionSyncPosition { get; set; }
}

public class ProjectionSyncPosition
{
    public long OriginalVersion { get; set; }
    public long CurrentVersion { get; set; }
}
