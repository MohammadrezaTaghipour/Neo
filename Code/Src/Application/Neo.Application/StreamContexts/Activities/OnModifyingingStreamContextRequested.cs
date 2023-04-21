using MassTransit;
using MassTransit.Courier.Contracts;
using Microsoft.Extensions.Options;
using Neo.Application.Contracts.ReferentialPointers;
using Neo.Application.Contracts.StreamContexts;
using Neo.Application.ReferentialPointers;

namespace Neo.Application.StreamContexts.Activities;

public class OnModifyingStreamContextRequested :
    IStateMachineActivity<StreamContextMachineState,
        ModifyingStreamContextRequested>
{
    private readonly MassTransitOptions _options;

    public OnModifyingStreamContextRequested(IOptions<MassTransitOptions> options)
    {
        _options = options.Value;
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(
        BehaviorContext<StreamContextMachineState,
        ModifyingStreamContextRequested> context,
        IBehavior<StreamContextMachineState,
            ModifyingStreamContextRequested> next)
    {
        UpdateReferentialPointersState(context.Saga, context.Message);

        var builder = new RoutingSlipBuilder(NewId.NextGuid());

        builder.AddActivity(nameof(ModifyStreamContextActivity),
            RoutingSlipAddress.ForQueue<ModifyStreamContextActivity,
                ModifyingStreamContextRequested>(),
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

        await builder.AddSubscription(
            new Uri(_options.StreamContextStateMachineAddress),
            RoutingSlipEvents.Completed | RoutingSlipEvents.Supplemental,
            RoutingSlipEventContents.Data,
            x => x.Send(new StreamContextActivitiesCompleted
            {
                Id = context.Message.Id
            }));

        var routingSlip = builder.Build();
        await context.Execute(routingSlip);

        await next.Execute(context).ConfigureAwait(false);
    }

    public Task Faulted<TException>(
        BehaviorExceptionContext<StreamContextMachineState,
        ModifyingStreamContextRequested, TException> context,
        IBehavior<StreamContextMachineState,
            ModifyingStreamContextRequested> next)
        where TException : Exception
    {
        return next.Faulted(context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("modifying-streamContext");
    }

    private static void UpdateReferentialPointersState(
        StreamContextMachineState machineState,
        ModifyingStreamContextRequested request)
    {
        machineState.ReferentialPointerCurrentState = machineState
             .ReferentialPointerNextState.Clone();
        machineState.ReferentialPointerNextState = new();

        foreach (var item in request.StreamEventTypes)
        {
            if (machineState.ReferentialPointerCurrentState.UsedItems
                .FirstOrDefault(_ => _.Id == item.StreamEventTypeId) == null)
            {
                machineState.ReferentialPointerNextState.UsedItems
                    .Add(new ReferentialStateRecord(
                        item.StreamEventTypeId,
                        ReferentialPointerType.StreamContext.ToString()));
            }
        }

        foreach (var item in machineState.ReferentialPointerCurrentState.UnusedItems)
        {
            if (request.StreamEventTypes
                .FirstOrDefault(_ => _.StreamEventTypeId == item.Id) == null)
            {
                machineState.ReferentialPointerNextState.UnusedItems
                    .Add(new ReferentialStateRecord(
                        item.Id,
                        ReferentialPointerType.StreamContext.ToString()));
            }
        }
    }
}
