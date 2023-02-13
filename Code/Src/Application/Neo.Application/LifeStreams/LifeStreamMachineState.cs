using MassTransit;
using Neo.Application.Contracts.ReferentialPointers;
using Neo.Infrastructure.Framework.AspCore;

namespace Neo.Application.LifeStreams;

public class LifeStreamMachineState :
    SagaStateMachineInstance,
    ISagaVersion
{
    public Guid CorrelationId { get; set; }
    public int Version { get; set; }
    public string CurrentState { get; set; }

    public Guid LifeStreamId { get; set; }
    public ErrorResponse Error { get; set; }

    public ReferentialPointerContainer ReferentialPointerCurrentState { get; set; }
    public ReferentialPointerContainer ReferentialPointerNextState { get; set; }
}