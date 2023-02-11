using MassTransit;
using Neo.Application.Contracts.LifeStreams;
using Neo.Application.Contracts;
using Neo.Application.LifeStreams.Activities;

namespace Neo.Application.LifeStreams;

public class LifeStreamStateMachine :
    MassTransitStateMachine<LifeStreamMachineState>
{
    public LifeStreamStateMachine()
    {
        Event(() => StatusRequested, x =>
        {
            x.CorrelateById(m => m.Message.Id);
            x.OnMissingInstance(m => m.ExecuteAsync(async context =>
            {
                if (context.RequestId.HasValue)
                {
                    await context.RespondAsync(
                        new LifeStreamStatusRequestExecuted
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
                .Activity(_ => _.OfType<OnDefiningLifeStream>()
                .RespondAsync(_ =>
                {
                    return _.Init<DefiningLifeStreamRequested>(_.Message);
                })
                .TransitionTo(Defining)));

        During(Defining,
            When(DefiningExecuted)
                .TransitionTo(ReferentialSyncing),
            When(ActivitiesFaulted)
                .Activity(_ => _.OfType<OnLifeStreamActivitiesFaulted>())
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
                .Activity(_ => _.OfType<OnModifyingLifeStream>()
                .RespondAsync(_ =>
                {
                    return _.Init<ModifyingLifeStreamRequested>(_.Message);
                })
                .TransitionTo(Modifying)),
            When(ModifyingExecuted)
                .TransitionTo(Idle),
            When(RemovingRequested)
                .Activity(_ => _.OfType<OnRemovingLifeStream>()
                .RespondAsync(_ =>
                {
                    return _.Init<RemovingLifeStreamRequested>(_.Message);
                })
                .TransitionTo(Removing)));

        During(Modifying,
            When(ModifyingExecuted)
                .TransitionTo(Idle),
            When(ActivitiesFaulted)
                .Activity(_ => _.OfType<OnLifeStreamActivitiesFaulted>())
                .TransitionTo(Idle));

        During(Removing,
            When(RemovingExecuted)
                .TransitionTo(ReferentialSyncing),
            When(ActivitiesFaulted)
                .Activity(_ => _.OfType<OnLifeStreamActivitiesFaulted>())
                .TransitionTo(Idle));

        DuringAny(
            When(StatusRequested)
                    .RespondAsync(x => x.Init<LifeStreamStatusRequestExecuted>(
                        new LifeStreamStatusRequestExecuted
                        {
                            Id = x.Saga.LifeStreamId,
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


    public Event<LifeStreamActivitiesCompleted> ActivitiesCompleted { get; private set; }
    public Event<ActivitiesFaulted> ActivitiesFaulted { get; private set; }
    public Event<LifeStreamStatusRequested> StatusRequested { get; private set; }
    public Event<DefiningLifeStreamRequested> DefiningRequested { get; private set; }
    public Event<DefiningLifeStreamRequestExecuted> DefiningExecuted { get; private set; }
    public Event<ModifyingLifeStreamRequested> ModifyingRequested { get; private set; }
    public Event<ModifyingLifeStreamRequestExecuted> ModifyingExecuted { get; private set; }
    public Event<RemovingLifeStreamRequested> RemovingRequested { get; private set; }
    public Event<RemovingLifeStreamRequestExecuted> RemovingExecuted { get; private set; }
}
