﻿using MassTransit;
using MassTransit.Courier.Contracts;
using Neo.Application.Contracts.ReferentialPointers;
using Neo.Application.Contracts.StreamEventTypes;
using Neo.Application.ReferentialPointers;
using Neo.Domain.Contracts.ReferentialPointers;

namespace Neo.Application.StreamEventTypes.Activities;

public class OnDefiningStreamEventTypeRequested :
    IStateMachineActivity<StreamEventTypeMachineState, DefiningStreamEventTypeRequested>
{
    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<StreamEventTypeMachineState,
        DefiningStreamEventTypeRequested> context,
        IBehavior<StreamEventTypeMachineState, DefiningStreamEventTypeRequested> next)
    {
        context.Saga.StreamEventTypeId = context.Message.Id;

        UpdateReferentialPointersState(context.Saga, context.Message);

        var builder = new RoutingSlipBuilder(NewId.NextGuid());

        builder.AddActivity(nameof(DefineStreamEventTypeActivity),
            RoutingSlipAddress.ForQueue<DefineStreamEventTypeActivity,
                DefiningStreamEventTypeRequested>(),
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
                x => x.Send(new StreamEventTypeActivityCompleted
                {
                    Id = context.Message.Id
                }));

        var routingSlip = builder.Build();
        await context.Execute(routingSlip);

        await next.Execute(context).ConfigureAwait(false);
    }

    public Task Faulted<TException>(BehaviorExceptionContext<StreamEventTypeMachineState,
        DefiningStreamEventTypeRequested, TException> context,
        IBehavior<StreamEventTypeMachineState, DefiningStreamEventTypeRequested> next)
        where TException : Exception
    {
        return next.Faulted(context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("defining-streamEventType");
    }

    private static void UpdateReferentialPointersState(
        StreamEventTypeMachineState machineState,
        DefiningStreamEventTypeRequested request)
    {
        machineState.ReferentialPointerNextState.DefinedItems
            .Add(new ReferentialStateRecord(request.Id,
                ReferentialPointerType.StreamContext.ToString()));
    }
}
