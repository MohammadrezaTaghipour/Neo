using MassTransit;
using Neo.Application.Contracts.ReferentialPointers;
using Neo.Infrastructure.Framework.AspCore;
using Neo.Infrastructure.Framework.Domain;
using Newtonsoft.Json;

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
    //TODO: this approach doesn't work well
    // in some situations a request might receive others error
    // so, think to find another solution.
    public ErrorResponse Error { get; set; }

    public ReferentialPointerContainer ReferentialPointerCurrentState { get; set; }
    public ReferentialPointerContainer ReferentialPointerNextState { get; set; }
    public ProjectionSyncPosition ProjectionSyncPosition { get; set; }
}

public class ProjectionSyncPosition
{
    public long OriginalVersion { get; set; } 
    public long CurrentVersion { get; set; }
}
