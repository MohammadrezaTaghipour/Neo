using MassTransit;
using Neo.Application.Contracts.ReferentialPointers;
using Neo.Application.Contracts.StreamEventTypes;
using Neo.Application.StreamEventTypes.Activities;

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
        Event(() => ReferentialPointersSynced, x => x.CorrelateById(m => m.Message.Id));

        Event(() => DefiningRequested, x => x.CorrelateById(m => m.Message.Id));
        Event(() => DefiningExecuted, x => x.CorrelateById(m => m.Message.Id));
        Event(() => DefiningFaulted, x => x.CorrelateById(m => m.Message.Id));
        Event(() => ModifyingRequested, x => x.CorrelateById(m => m.Message.Id));
        Event(() => ModifyingExecuted, x => x.CorrelateById(m => m.Message.Id));
        Event(() => ModifyingFaulted, x => x.CorrelateById(m => m.Message.Id));
        Event(() => RemovingRequested, x => x.CorrelateById(m => m.Message.Id));
        Event(() => RemovingExecuted, x => x.CorrelateById(m => m.Message.Id));
        Event(() => RemovingFaulted, x => x.CorrelateById(m => m.Message.Id));

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
                })
                .TransitionTo(Faulted));

        During(ReferentialSyncing,
            When(ReferentialPointersSynced)
                .TransitionTo(Idle));

        During(Idle,
            When(ModifyingRequested)
                .Activity(_ => _.OfType<OnModifyingStreamEventTypeRequested>()
                .TransitionTo(Modifying)),
            When(RemovingRequested)
                .Activity(_ => _.OfType<OnRemovingStreamEventTypeRequested>()
                .TransitionTo(Removing)));

        During(Modifying,
            When(ModifyingExecuted)
                .TransitionTo(Idle),
            When(ModifyingFaulted)
                .Then(_ =>
                {
                    _.Saga.ErrorCode = _.Message.ErrorCode;
                    _.Saga.ErrorMessage = _.Message.ErrorMessage;
                })
                .TransitionTo(Idle));

        During(Removing,
            When(RemovingExecuted)
                .TransitionTo(ReferentialSyncing),
            When(RemovingFaulted)
                .Then(_ =>
                {
                    _.Saga.ErrorCode = _.Message.ErrorCode;
                    _.Message.ErrorMessage = _.Message.ErrorMessage;
                })
                .TransitionTo(Idle));

        DuringAny(
            When(StatusRequested)
                    .RespondAsync(x => x.Init<StreamEventTypeStatusRequestExecuted>(
                        new StreamEventTypeStatusRequestExecuted
                        {
                            Id = x.Saga.StreamEventTypeId,
                            Completed = x.Saga.CurrentState == nameof(Idle) ||
                                        x.Saga.CurrentState == nameof(Faulted),
                            ErrorCode = x.Saga.ErrorCode,
                            ErrorMessage = x.Saga.ErrorMessage
                        })));
    }


    public State ReferentialSyncing { get; private set; }
    public State Defining { get; private set; }
    public State Modifying { get; private set; }
    public State Removing { get; private set; }
    public State Idle { get; private set; }
    public State Faulted { get; private set; }


    public Event<StreamEventTypeStatusRequested> StatusRequested { get; private set; }
    public Event<ReferentialPointersSynced> ReferentialPointersSynced { get; private set; }
    public Event<DefiningStreamEventTypeRequested> DefiningRequested { get; private set; }
    public Event<DefiningStreamEventTypeRequestExecuted> DefiningExecuted { get; private set; }
    public Event<DefiningStreamEventTypeFaulted> DefiningFaulted { get; private set; }
    public Event<ModifyingStreamEventTypeRequested> ModifyingRequested { get; private set; }
    public Event<ModifyingStreamEventTypeRequestExecuted> ModifyingExecuted { get; private set; }
    public Event<ModifyingStreamEventTypeFaulted> ModifyingFaulted { get; private set; }
    public Event<RemovingStreamEventTypeRequested> RemovingRequested { get; private set; }
    public Event<RemovingStreamEventTypeRequestExecuted> RemovingExecuted { get; private set; }
    public Event<RemovingStreamEventTypeFaulted> RemovingFaulted { get; private set; }
}
