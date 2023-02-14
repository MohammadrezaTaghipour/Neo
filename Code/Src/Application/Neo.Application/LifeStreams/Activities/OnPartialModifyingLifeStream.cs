using MassTransit.Courier.Contracts;
using MassTransit;
using Neo.Application.Contracts.LifeStreams;
using Neo.Application.Contracts.ReferentialPointers;
using Neo.Application.ReferentialPointers;

namespace Neo.Application.LifeStreams.Activities;

public class OnPartialModifyingLifeStream :
    IStateMachineActivity<LifeStreamMachineState, PartialModifyingLifeStreamRequested>
{
    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(
        BehaviorContext<LifeStreamMachineState,
        PartialModifyingLifeStreamRequested> context,
        IBehavior<LifeStreamMachineState, PartialModifyingLifeStreamRequested> next)
    {
        UpdateReferentialPointersState(context.Saga, context.Message);

        var builder = new RoutingSlipBuilder(NewId.NextGuid());

        switch (context.Message.OperationType)
        {
            case LifeStreamPartialModificationOperationType.AppendStreamEvent:
                builder.AddActivity(nameof(AppendStreamEventActivity),
                RoutingSlipAddress.ForQueue<AppendStreamEventActivity,
                AppendngStreamEventRequested>(),
                new AppendngStreamEventRequested
                {
                    LifeStreamId = context.Message.LifeStreamId,
                    StreamContextId = context.Message.StreamContextId,
                    StreamEventTypeId = context.Message.StreamEventTypeId,
                    Metadata = context.Message.Metadata,
                });
                break;

            case LifeStreamPartialModificationOperationType.RemoveStreamEvent:
                builder.AddActivity(nameof(RemoveStreamEventActivity),
                RoutingSlipAddress.ForQueue<RemoveStreamEventActivity,
                RemovingStreamEventRequested>(),
                new RemovingStreamEventRequested
                {
                    Id = context.Message.Id,
                    LifeStreamId = context.Message.LifeStreamId,
                });
                break;
        }

        builder.AddActivity(nameof(SyncReferentialPointersActivity),
            RoutingSlipAddress.ForQueue<SyncReferentialPointersActivity,
                SyncingReferentialPointersRequested>(),
            new SyncingReferentialPointersRequested
            {
                Id = context.Message.LifeStreamId,
                CurrentState = context.Saga.ReferentialPointerCurrentState,
                NextState = context.Saga.ReferentialPointerNextState
            });

        await builder.AddSubscription(new Uri("queue:life-stream-machine-state"),
                RoutingSlipEvents.Completed,
                RoutingSlipEventContents.Data,
                x => x.Send(new LifeStreamActivitiesCompleted
                {
                    Id = context.Message.LifeStreamId
                }));

        var routingSlip = builder.Build();
        await context.Execute(routingSlip).ConfigureAwait(false);

        await next.Execute(context).ConfigureAwait(false);
    }

    public Task Faulted<TException>(
        BehaviorExceptionContext<LifeStreamMachineState,
        PartialModifyingLifeStreamRequested, TException> context,
        IBehavior<LifeStreamMachineState, PartialModifyingLifeStreamRequested> next)
        where TException : Exception
    {
        return next.Faulted(context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("partialModifying-lifeStream");
    }

    private static void UpdateReferentialPointersState(
        LifeStreamMachineState machineState,
        PartialModifyingLifeStreamRequested request)
    {
        machineState.ReferentialPointerCurrentState = machineState
            .ReferentialPointerNextState.Clone();
        machineState.ReferentialPointerNextState = new();

        if (request.OperationType ==
            LifeStreamPartialModificationOperationType.AppendStreamEvent)
        {
            machineState.ReferentialPointerNextState.UsedItems
                .Add(new ReferentialStateRecord(request.StreamEventTypeId,
                ReferentialPointerType.LifeStream.ToString()));

            machineState.ReferentialPointerNextState.UsedItems
                .Add(new ReferentialStateRecord(request.StreamContextId,
                ReferentialPointerType.LifeStream.ToString()));
        }
        else if (request.OperationType ==
            LifeStreamPartialModificationOperationType.RemoveStreamEvent)
        {
            foreach (var item in machineState.ReferentialPointerCurrentState.UsedItems)
            {
                machineState.ReferentialPointerNextState.UnusedItems
                .Add(new ReferentialStateRecord(item.Id,
                ReferentialPointerType.LifeStream.ToString()));
            }
        }
    }

}