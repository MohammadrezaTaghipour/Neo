using MassTransit;
using Neo.Application.Contracts.StreamContexts;
using Neo.Application.StreamContexts.StateMachineActivities;

namespace Neo.Application.StreamContexts;

public class StreamContextStateMachine :
    MassTransitStateMachine<StreamContextState>
{
    public StreamContextStateMachine()
    {
        Event(() => DefiningRequested, x => x.CorrelateById(m => m.Message.Id));
        Event(() => DefiningExecuted, x => x.CorrelateById(m => m.Message.Id));
        Event(() => DefiningFaulted, x => x.CorrelateById(m => m.Message.Message.Id));
        Event(() => ReferentialPointersSynced, x => x.CorrelateById(m => m.Message.Id));

        Initially(
            When(DefiningRequested)
                .Activity(_ => _.OfType<OnDefiningStreamContextRequested>()
                .Then(_ =>
                {
                    _.Saga.StreamEventTypeIds = _.Message.StreamEventTypes
                        .Select(_ => _.StreamEventTypeId).ToList();
                })
                .TransitionTo(Defining)));

        During(Defining,
            When(DefiningExecuted)
                .Then(_ => { })
                .TransitionTo(ReferentialSyncing),
            When(DefiningFaulted)
                .Finalize());

        During(ReferentialSyncing,
            When(ReferentialPointersSynced)
                .Then(_ => { })
                .TransitionTo(Idle));
    }


    public State Defining { get; set; }
    public State Defined { get; set; }
    public State ReferentialSyncing { get; set; }
    public State Idle { get; set; }

    public Event<DefiningStreamContextRequested> DefiningRequested { get; set; }
    public Event<DefiningStreamContextRequestExecuted> DefiningExecuted { get; set; }
    public Event<Fault<DefiningStreamContextRequested>> DefiningFaulted { get; set; }
    public Event<StreamContextReferentialPointersSynced> ReferentialPointersSynced { get; set; }



}


public class StreamContextState :
    SagaStateMachineInstance,
    ISagaVersion
{
    public Guid CorrelationId { get; set; }
    public int Version { get; set; }
    public string CurrentState { get; set; }

    public List<Guid> StreamEventTypeIds { get; set; }
}