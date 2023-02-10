﻿using MassTransit;
using Neo.Application.Contracts.ReferentialPointers;
using Neo.Application.Contracts.StreamContexts;
using Neo.Application.ReferentialPointers;

namespace Neo.Application.StreamContexts.Activities;

public class OnModifyingStreamContextRequested :
    IStateMachineActivity<StreamContextMachineState, ModifyingStreamContextRequested>
{
    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<StreamContextMachineState,
        ModifyingStreamContextRequested> context,
        IBehavior<StreamContextMachineState, ModifyingStreamContextRequested> next)
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

        var routingSlip = builder.Build();
        await context.Execute(routingSlip);

        await next.Execute(context).ConfigureAwait(false);
    }

    public Task Faulted<TException>(BehaviorExceptionContext<StreamContextMachineState,
        ModifyingStreamContextRequested, TException> context,
        IBehavior<StreamContextMachineState, ModifyingStreamContextRequested> next)
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
        var currentState = machineState.ReferentialPointerCurrentState;
        var nextState = machineState.ReferentialPointerNextState;

        currentState = nextState.Clone();
        nextState = new();

        foreach (var item in request.StreamEventTypes)
        {
            if (currentState.UsedItems
                .FirstOrDefault(_ => _.Id == item.StreamEventTypeId) == null)
            {
                nextState.UsedItems.Add(new ReferentialStateRecord(
                    item.StreamEventTypeId,
                    ReferentialPointerType.StreamContext.ToString()));
            }
        }

        foreach (var item in currentState.UnusedItems)
        {
            if (request.StreamEventTypes
                .FirstOrDefault(_ => _.StreamEventTypeId == item.Id) == null)
            {
                nextState.UnusedItems.Add(new ReferentialStateRecord(
                    item.Id,
                    ReferentialPointerType.StreamContext.ToString()));
            }
        }
    }
}