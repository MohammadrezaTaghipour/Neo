using MassTransit;
using Neo.Application.Contracts;
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
                    await context.RespondAsync(
                        new StreamEventTypeStatusRequestExecuted
                        {
                            Faulted = true,
                            ErrorMessage = $"Item with id:'{context.Message.Id}' not found."
                        });
                }
            }));
        });
        Event(() => ActivitiesCompleted, x => x.CorrelateById(m => m.Message.Id));
        Event(() => ActivitiesFaulted, x => x.CorrelateById(m => m.Message.Id));
        Event(() => DefiningRequested, x => x.CorrelateById(m => m.Message.Id));
        Event(() => DefiningExecuted, x => x.CorrelateById(m => m.Message.Id));
        Event(() => ModifyingRequested, x => x.CorrelateById(m => m.Message.Id));
        Event(() => ModifyingExecuted, x => x.CorrelateById(m => m.Message.Id));
        Event(() => RemovingRequested, x => x.CorrelateById(m => m.Message.Id));
        Event(() => RemovingExecuted, x => x.CorrelateById(m => m.Message.Id));

        InstanceState(x => x.CurrentState);

        Initially(
            When(DefiningRequested)
                .Activity(_ => _.OfType<OnDefiningStreamEventTypeRequested>()
                .RespondAsync(_ =>
                {
                    return _.Init<DefiningStreamEventTypeRequested>(_.Message);
                })
                .TransitionTo(Defining)));

        During(Defining,
            When(DefiningExecuted)
                .TransitionTo(ReferentialSyncing),
            When(ActivitiesFaulted)
                .Activity(_ => _.OfType<OnStreamEventTypeActivitiesFaulted>())
                .TransitionTo(Faulted),
            When(ActivitiesCompleted)
                .TransitionTo(Idle));

        During(ReferentialSyncing,
            When(ActivitiesCompleted)
                .TransitionTo(Idle));

        During(Idle,
            Ignore(DefiningExecuted),
            Ignore(RemovingExecuted),
            Ignore(ActivitiesCompleted),
            When(ModifyingRequested)
                .Activity(_ => _.OfType<OnModifyingStreamEventTypeRequested>()
                .RespondAsync(_ =>
                {
                    return _.Init<ModifyingStreamEventTypeRequested>(_.Message);
                })
                .TransitionTo(Modifying)),
            When(ModifyingExecuted)
                .TransitionTo(Idle),
            When(RemovingRequested)
                .Activity(_ => _.OfType<OnRemovingStreamEventTypeRequested>()
                .RespondAsync(_ =>
                {
                    return _.Init<RemovingStreamEventTypeRequested>(_.Message);
                })
                .TransitionTo(Removing)));

        During(Modifying,
            When(ModifyingExecuted)
                .TransitionTo(Idle),
            When(ActivitiesFaulted)
                .Activity(_ => _.OfType<OnStreamEventTypeActivitiesFaulted>())
                .TransitionTo(Idle));

        During(Removing,
            When(RemovingExecuted)
                .TransitionTo(ReferentialSyncing),
            When(ActivitiesFaulted)
                .Activity(_ => _.OfType<OnStreamEventTypeActivitiesFaulted>())
                .TransitionTo(Idle));

        DuringAny(
            When(StatusRequested)
                    .RespondAsync(x => x.Init<StreamEventTypeStatusRequestExecuted>(
                        new StreamEventTypeStatusRequestExecuted
                        {
                            Id = x.Saga.StreamEventTypeId,
                            Completed = x.Saga.CurrentState == nameof(Idle) ||
                                        x.Saga.CurrentState == nameof(Faulted),
                            Faulted = x.Saga.Error != null,
                            ErrorCode = x.Saga.Error?.Code,
                            ErrorMessage = x.Saga.Error?.Message
                        })));
    }


    public State ReferentialSyncing { get; private set; }
    public State Defining { get; private set; }
    public State Modifying { get; private set; }
    public State Removing { get; private set; }
    public State Idle { get; private set; }
    public State Faulted { get; private set; }


    public Event<StreamEventTypeActivitiesCompleted> ActivitiesCompleted { get; private set; }
    public Event<ActivitiesFaulted> ActivitiesFaulted { get; private set; }
    public Event<StreamEventTypeStatusRequested> StatusRequested { get; private set; }
    public Event<DefiningStreamEventTypeRequested> DefiningRequested { get; private set; }
    public Event<DefiningStreamEventTypeRequestExecuted> DefiningExecuted { get; private set; }
    public Event<ModifyingStreamEventTypeRequested> ModifyingRequested { get; private set; }
    public Event<ModifyingStreamEventTypeRequestExecuted> ModifyingExecuted { get; private set; }
    public Event<RemovingStreamEventTypeRequested> RemovingRequested { get; private set; }
    public Event<RemovingStreamEventTypeRequestExecuted> RemovingExecuted { get; private set; }
}
