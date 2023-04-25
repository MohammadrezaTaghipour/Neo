using MassTransit;
using Neo.Application.Contracts;
using Neo.Application.Contracts.ReferentialPointers;
using Neo.Application.Contracts.StreamContexts;
using Neo.Application.StreamContexts.Activities;

namespace Neo.Application.StreamContexts;

public class StreamContextStateMachine :
    MassTransitStateMachine<StreamContextMachineState>
{
    public StreamContextStateMachine()
    {
        Event(() => StatusRequested, x =>
        {
            x.CorrelateById(m => m.Message.Id);
            x.OnMissingInstance(m => m.ExecuteAsync(async context =>
            {
                if (context.RequestId.HasValue)
                {
                    await context.RespondAsync(new StreamContextStatusRequestExecuted
                    {
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
        Event(() => SyncingReferentialPointersExecuted, x => x.CorrelateById(m => m.Message.Id));

        InstanceState(x => x.CurrentState);

        Initially(
            When(DefiningRequested)
                .Activity(_ => _.OfType<OnDefiningStreamContextRequested>()
                .RespondAsync(_ =>
                {
                    return _.Init<DefiningStreamContextRequested>(_.Message);
                })
                .TransitionTo(Defining)));

        During(Defining,
            When(DefiningExecuted)
                .TransitionTo(ReferentialSyncing),
            When(ActivitiesCompleted)
                .Activity(_ => _.OfType<OnStreamContextActivitiesCompleted>())
                .TransitionTo(Idle),
            When(ActivitiesFaulted)
                .Activity(_ => _.OfType<OnStreamContextActivitiesFaulted>())
                .Finalize());

        During(ReferentialSyncing,
            When(SyncingReferentialPointersExecuted)
                .TransitionTo(ProjectionSyncing),
            When(ActivitiesCompleted)
                .Activity(_ => _.OfType<OnStreamContextActivitiesCompleted>())
                .TransitionTo(Idle));

        During(ProjectionSyncing,
            When(ActivitiesCompleted)
                .Activity(_ => _.OfType<OnStreamContextActivitiesCompleted>())
                .TransitionTo(Idle));

        During(Idle,
            Ignore(DefiningExecuted),
            Ignore(RemovingExecuted),
            Ignore(ActivitiesCompleted),
            When(ModifyingRequested)
                .Activity(_ => _.OfType<OnModifyingStreamContextRequested>()
                .RespondAsync(_ =>
                {
                    return _.Init<ModifyingStreamContextRequested>(_.Message);
                })
                .TransitionTo(Modifying)),
            When(ModifyingExecuted)
                .TransitionTo(ReferentialSyncing),
            When(RemovingRequested)
                .Activity(_ => _.OfType<OnRemovingStreamContextRequested>()
                .RespondAsync(_ =>
                {
                    return _.Init<RemovingStreamContextRequested>(_.Message);
                })
                .TransitionTo(Removing)));

        During(Modifying,
            When(ModifyingExecuted)
                .TransitionTo(ReferentialSyncing),
            When(ActivitiesFaulted)
                .Activity(_ => _.OfType<OnStreamContextActivitiesFaulted>())
                .TransitionTo(Idle));

        During(Removing,
            When(RemovingExecuted)
                .TransitionTo(ReferentialSyncing),
            When(ActivitiesFaulted)
                .Activity(_ => _.OfType<OnStreamContextActivitiesFaulted>())
                .TransitionTo(Idle));

        DuringAny(
            When(StatusRequested)
                .RespondAsync(x => x.Init<StreamContextStatusRequestExecuted>(
                    new StreamContextStatusRequestExecuted
                    {
                        Id = x.Saga.StreamContextId,
                        Completed = x.Saga.CurrentState == nameof(Idle),
                    })));
    }


    public State Defining { get; private set; }
    public State Modifying { get; private set; }
    public State Removing { get; private set; }
    public State Idle { get; private set; }
    public State ReferentialSyncing { get; private set; }
    public State ProjectionSyncing { get; private set; }

    public Event<StreamContextStatusRequested> StatusRequested { get; private set; }
    public Event<StreamContextActivitiesCompleted> ActivitiesCompleted { get; private set; }
    public Event<ActivitiesFaulted> ActivitiesFaulted { get; private set; }
    public Event<DefiningStreamContextRequested> DefiningRequested { get; private set; }
    public Event<DefiningStreamContextRequestExecuted> DefiningExecuted { get; private set; }
    public Event<ModifyingStreamContextRequested> ModifyingRequested { get; private set; }
    public Event<ModifyingStreamContextRequestExecuted> ModifyingExecuted { get; private set; }
    public Event<RemovingStreamContextRequested> RemovingRequested { get; private set; }
    public Event<RemovingStreamContextRequestExecuted> RemovingExecuted { get; private set; }
    public Event<SyncingReferentialPointersRequestExecuted> SyncingReferentialPointersExecuted { get; private set; }
}
