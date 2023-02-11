using MassTransit;
using MassTransit.Courier.Contracts;
using Neo.Application.Contracts.ReferentialPointers;
using Neo.Application.Contracts.StreamContexts;
using Neo.Application.ReferentialPointers;

namespace Neo.Application.StreamContexts.Activities;

public class OnRemovingStreamContextRequested :
    IStateMachineActivity<StreamContextMachineState, RemovingStreamContextRequested>
{
    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<StreamContextMachineState,
        RemovingStreamContextRequested> context,
        IBehavior<StreamContextMachineState, RemovingStreamContextRequested> next)
    {
        context.Saga.StreamContextId = context.Message.Id;

        UpdateReferentialPointersState(context.Saga, context.Message);

        var builder = new RoutingSlipBuilder(NewId.NextGuid());

        builder.AddActivity(nameof(RemoveStreamContextActivity),
            RoutingSlipAddress.ForQueue<RemoveStreamContextActivity,
                RemovingStreamContextRequested>(),
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

        await builder.AddSubscription(new Uri("queue:stream-context-machine-state"),
                RoutingSlipEvents.Completed,
                RoutingSlipEventContents.Data,
                x => x.Send(new StreamContextActivitiesCompleted
                {
                    Id = context.Message.Id
                }));

        var routingSlip = builder.Build();
        await context.Execute(routingSlip);

        await next.Execute(context).ConfigureAwait(false);
    }

    public Task Faulted<TException>(BehaviorExceptionContext<StreamContextMachineState,
        RemovingStreamContextRequested, TException> context,
        IBehavior<StreamContextMachineState, RemovingStreamContextRequested> next)
        where TException : Exception
    {
        return next.Faulted(context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("removing-streamContext");
    }

    private static void UpdateReferentialPointersState(
        StreamContextMachineState machineState,
        RemovingStreamContextRequested request)
    {
        machineState.ReferentialPointerCurrentState = machineState
            .ReferentialPointerNextState.Clone();
        machineState.ReferentialPointerNextState = new();

        machineState.ReferentialPointerNextState.RemovedItems
            .Add(new ReferentialStateRecord(request.Id,
                ReferentialPointerType.StreamContext.ToString()));

        foreach (var item in machineState.ReferentialPointerCurrentState.UsedItems)
        {
            machineState.ReferentialPointerNextState.UnusedItems
                .Add(new ReferentialStateRecord(
                    item.Id,
                    ReferentialPointerType.StreamContext.ToString()));
        }
    }
}
