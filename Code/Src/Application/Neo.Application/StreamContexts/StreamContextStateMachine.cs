using MassTransit;
using Neo.Application.Contracts.ReferentialPointers;
using Neo.Application.Contracts.StreamContexts;
using Neo.Application.Contracts.StreamContexts;
using Neo.Application.StreamContexts.Activities;
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
                .Activity(_ => _.OfType<OnDefiningStreamContextRequested>()
                .TransitionTo(Defining)));

        During(Defining,
            When(DefiningExecuted)
                .TransitionTo(ReferentialSyncing),
            When(DefiningFaulted)
                .Then(_ =>
                {
                    _.Saga.ErrorCode = _.Message.ErrorCode;
                    _.Saga.ErrorMessage = _.Message.ErrorMessage;
                })
                .TransitionTo(Faulted));

        During(ReferentialSyncing,
            When(ReferentialPointersSynced)
                .TransitionTo(Idle));

        During(Idle,
            When(ModifyingRequested)
                .Activity(_ => _.OfType<OnModifyingStreamContextRequested>()
                .TransitionTo(Modifying)),
            When(RemovingRequested)
                //.Activity(_ => _.OfType<OnRemovingStreamContextRequested>()
                .TransitionTo(Removing));

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
                    .RespondAsync(x => x.Init<StreamContextStatusRequestExecuted>(
                        new StreamContextStatusRequestExecuted
                        {
                            Id = x.Saga.StreamContextId,
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

    public Event<StreamContextStatusRequested> StatusRequested { get; private set; }
    public Event<ReferentialPointersSynced> ReferentialPointersSynced { get; private set; }
    public Event<DefiningStreamContextRequested> DefiningRequested { get; private set; }
    public Event<DefiningStreamContextRequestExecuted> DefiningExecuted { get; private set; }
    public Event<DefiningStreamContextFaulted> DefiningFaulted { get; private set; }
    public Event<ModifyingStreamContextRequested> ModifyingRequested { get; private set; }
    public Event<ModifyingStreamContextRequestExecuted> ModifyingExecuted { get; private set; }
    public Event<ModifyingStreamContextFaulted> ModifyingFaulted { get; private set; }
    public Event<RemovingStreamContextRequested> RemovingRequested { get; private set; }
    public Event<RemovingStreamContextRequestExecuted> RemovingExecuted { get; private set; }
    public Event<RemovingStreamContextFaulted> RemovingFaulted { get; private set; }

}
