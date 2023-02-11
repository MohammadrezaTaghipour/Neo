using MassTransit;
using MassTransit.Courier.Contracts;
using Neo.Application.Contracts.ReferentialPointers;
using Neo.Application.Contracts.StreamEventTypes;
using Neo.Application.ReferentialPointers;

namespace Neo.Application.StreamEventTypes.Activities;

public class OnRemovingStreamEventTypeRequested :
    IStateMachineActivity<StreamEventTypeMachineState, RemovingStreamEventTypeRequested>
{
    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<StreamEventTypeMachineState,
        RemovingStreamEventTypeRequested> context,
        IBehavior<StreamEventTypeMachineState, RemovingStreamEventTypeRequested> next)
    {
        UpdateReferentialPointersState(context.Saga, context.Message);

        var builder = new RoutingSlipBuilder(NewId.NextGuid());

        builder.AddActivity(nameof(RemoveStreamEventTypeActivity),
            RoutingSlipAddress.ForQueue<RemoveStreamEventTypeActivity,
                RemovingStreamEventTypeRequested>(),
            context.Message);

        builder.AddActivity(nameof(SyncReferentialPointersActivity),
            RoutingSlipAddress.ForQueue<SyncReferentialPointersActivity,
                SyncingReferentialPointersRequested>(),
            new SyncingReferentialPointersRequested
            {
                Id = context.Message.Id,
                CurrentState = context.Saga.ReferentialPointerCurrentState,
                NextState = context.Saga.ReferentialPointerNextState
            });

        await builder.AddSubscription(new Uri("queue:stream-event-type-machine-state"),
                 RoutingSlipEvents.Completed,
                 RoutingSlipEventContents.Data,
                 x => x.Send(new StreamEventTypeActivitiesCompleted
                 {
                     Id = context.Message.Id
                 }));

        var routingSlip = builder.Build();
        await context.Execute(routingSlip);

        await next.Execute(context).ConfigureAwait(false);
    }

    public Task Faulted<TException>(BehaviorExceptionContext<StreamEventTypeMachineState,
        RemovingStreamEventTypeRequested, TException> context,
        IBehavior<StreamEventTypeMachineState, RemovingStreamEventTypeRequested> next)
        where TException : Exception
    {
        return next.Faulted(context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("removing-streamEventType");
    }

    private static void UpdateReferentialPointersState(
        StreamEventTypeMachineState machineState,
        RemovingStreamEventTypeRequested request)
    {
        machineState.ReferentialPointerNextState = new();
        machineState.ReferentialPointerNextState.RemovedItems
            .Add(new ReferentialStateRecord(request.Id,
                ReferentialPointerType.StreamEventType.ToString()));
    }
}