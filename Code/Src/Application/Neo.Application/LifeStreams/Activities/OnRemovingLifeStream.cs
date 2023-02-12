using MassTransit.Courier.Contracts;
using MassTransit;
using Neo.Application.Contracts.ReferentialPointers;
using Neo.Application.Contracts.LifeStreams;
using Neo.Application.ReferentialPointers;

namespace Neo.Application.LifeStreams.Activities;

public class OnRemovingLifeStream :
    IStateMachineActivity<LifeStreamMachineState, RemovingLifeStreamRequested>
{
    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<LifeStreamMachineState,
        RemovingLifeStreamRequested> context,
        IBehavior<LifeStreamMachineState, RemovingLifeStreamRequested> next)
    {
        context.Saga.LifeStreamId = context.Message.Id;

        UpdateReferentialPointersState(context.Saga, context.Message);

        var builder = new RoutingSlipBuilder(NewId.NextGuid());

        builder.AddActivity(nameof(RemoveLifeStreamActivity),
            RoutingSlipAddress.ForQueue<RemoveLifeStreamActivity,
                RemovingLifeStreamRequested>(),
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

        await builder.AddSubscription(new Uri("queue:life-stream-machine-state"),
                RoutingSlipEvents.Completed,
                RoutingSlipEventContents.Data,
                x => x.Send(new LifeStreamActivitiesCompleted
                {
                    Id = context.Message.Id
                }));

        var routingSlip = builder.Build();
        await context.Execute(routingSlip).ConfigureAwait(false);

        await next.Execute(context).ConfigureAwait(false);
    }

    public Task Faulted<TException>(BehaviorExceptionContext<LifeStreamMachineState,
        RemovingLifeStreamRequested, TException> context,
        IBehavior<LifeStreamMachineState, RemovingLifeStreamRequested> next)
        where TException : Exception
    {
        return next.Faulted(context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("removing-lifeStream");
    }

    private static void UpdateReferentialPointersState(
        LifeStreamMachineState machineState,
        RemovingLifeStreamRequested request)
    {
        machineState.ReferentialPointerCurrentState = machineState
            .ReferentialPointerNextState.Clone();
        machineState.ReferentialPointerNextState = new();

        machineState.ReferentialPointerNextState.RemovedItems
            .Add(new ReferentialStateRecord(request.Id,
                ReferentialPointerType.LifeStream.ToString()));
    }
}
