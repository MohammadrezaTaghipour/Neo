using MassTransit;
using Neo.Application.Contracts.StreamContexts;
using Neo.Application.StreamContexts.StateMachineActivities;
using Neo.Infrastructure.Framework.ReferentialPointers;

namespace Neo.Application.StreamContexts;

public class StreamContextStateMachine :
    MassTransitStateMachine<StreamContextMachineState>
{
    public StreamContextStateMachine()
    {
        Event(() => DefiningRequested, x => x.CorrelateById(m => m.Message.Id));
        Event(() => DefiningExecuted, x => x.CorrelateById(m => m.Message.Id));
        Event(() => DefiningFaulted, x => x.CorrelateById(m => m.Message.Id));
        Event(() => ReferentialPointersSynced, x => x.CorrelateById(m => m.Message.Id));

        InstanceState(x => x.CurrentState);

        Initially(
            When(DefiningRequested)
                .Activity(_ => _.OfType<OnDefiningStreamContextRequested>()
                .TransitionTo(Defining)));

        During(Defining,
            When(DefiningExecuted)
                .TransitionTo(ReferentialSyncing),
            When(DefiningFaulted)
                .Finalize());

        During(ReferentialSyncing,
            When(ReferentialPointersSynced)
                .Then(_ => { })
                .TransitionTo(Idle));

        SetCompletedWhenFinalized();
    }


    public State Defining { get; set; }
    public State Defined { get; set; }
    public State ReferentialSyncing { get; set; }
    public State Idle { get; set; }

    public Event<DefiningStreamContextRequested> DefiningRequested { get; set; }
    public Event<DefiningStreamContextRequestExecuted> DefiningExecuted { get; set; }
    public Event<DefiningStreamContextFaulted> DefiningFaulted { get; set; }
    public Event<StreamContextReferentialPointersSynced> ReferentialPointersSynced { get; set; }

}


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

    public ReferentialPointerContainer ReferentialPointerCurrentState { get; set; }
    public ReferentialPointerContainer ReferentialPointerNextState { get; set; }
}