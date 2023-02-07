using MassTransit;
using Neo.Application.Contracts.ReferentialPointers;
using Neo.Application.Contracts.StreamEventTypes;
using Neo.Application.StreamEventTypes.Activities;
using Neo.Infrastructure.Framework.ReferentialPointers;

namespace Neo.Application.StreamEventTypes;

public class StreamEventTypeStateMachine :
    MassTransitStateMachine<StreamEventTypeMachineState>
{
    public StreamEventTypeStateMachine()
    {
        Event(() => StatusRequested, x =>
        {
            x.CorrelateById(m => m.Message.Id);
            x.OnMissingInstance(m => m.ExecuteAsync(async context =>
            {
                if (context.RequestId.HasValue)
                {
                    await context.RespondAsync(new StreamEventTypeStatusRequestExecuted
                    {
                        Id = null,
                        ErrorMessage = $"Item With id:{context.Message.Id} not found."
                    });
                }
            }));
        });
        Event(() => DefiningRequested, x => x.CorrelateById(m => m.Message.Id));
        Event(() => DefiningExecuted, x => x.CorrelateById(m => m.Message.Id));
        Event(() => DefiningFaulted, x => x.CorrelateById(m => m.Message.Id));
        Event(() => ReferentialPointersSynced, x => x.CorrelateById(m => m.Message.Id));

        InstanceState(x => x.CurrentState);

        Initially(
            When(DefiningRequested)
                .Activity(_ => _.OfType<OnDefiningStreamEventTypeRequested>()
                .TransitionTo(Defining)));

        During(Defining,
            When(DefiningExecuted)
                .TransitionTo(ReferentialSyncing),
            When(DefiningFaulted)
                .Then(_ =>
                {
                    _.Saga.ErrorCode = _.Message.ErrorCode;
                    _.Message.ErrorMessage = _.Message.ErrorMessage;
                }));

        During(ReferentialSyncing,
            When(ReferentialPointersSynced)
                .TransitionTo(Idle));

        DuringAny(
            When(StatusRequested)
                    .RespondAsync(x => x.Init<StreamEventTypeStatusRequestExecuted>(
                        new StreamEventTypeStatusRequestExecuted
                        {
                            Id = x.Saga.StreamEventTypeId,
                            Completed = x.Saga.CurrentState == nameof(Idle),
                            ErrorCode = x.Saga.ErrorCode,
                            ErrorMessage = x.Saga.ErrorMessage
                        })));
    }

    public State Defining { get; private set; }
    public State Defined { get; private set; }
    public State ReferentialSyncing { get; private set; }
    public State Idle { get; private set; }

    public Event<StreamEventTypeStatusRequested> StatusRequested { get; private set; }

    public Event<DefiningStreamEventTypeRequested> DefiningRequested { get; private set; }
    public Event<DefiningStreamEventTypeRequestExecuted> DefiningExecuted { get; private set; }
    public Event<DefiningStreamEventTypeFaulted> DefiningFaulted { get; private set; }
    public Event<ReferentialPointersSynced> ReferentialPointersSynced { get; private set; }
}


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
