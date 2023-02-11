﻿using MassTransit.Courier.Contracts;
using MassTransit;
using Neo.Application.Contracts.LifeStreams;

namespace Neo.Application.LifeStreams.Activities;

public class OnModifyingLifeStream :
    IStateMachineActivity<LifeStreamMachineState, ModifyingLifeStreamRequested>
{
    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<LifeStreamMachineState,
        ModifyingLifeStreamRequested> context,
        IBehavior<LifeStreamMachineState, ModifyingLifeStreamRequested> next)
    {
        var builder = new RoutingSlipBuilder(NewId.NextGuid());

        builder.AddActivity(nameof(ModifyLifeStreamActivity),
            RoutingSlipAddress.ForQueue<ModifyLifeStreamActivity,
                ModifyingLifeStreamRequested>(),
            context.Message);


        await builder.AddSubscription(new Uri("queue:life-stream-machine-state"),
        RoutingSlipEvents.Completed,
        RoutingSlipEventContents.Data,
        x => x.Send(new LifeStreamActivitiesCompleted
        {
            Id = context.Message.Id
        }));

        var routingSlip = builder.Build();
        await context.Execute(routingSlip);

        await next.Execute(context).ConfigureAwait(false);
    }

    public Task Faulted<TException>(BehaviorExceptionContext<LifeStreamMachineState,
        ModifyingLifeStreamRequested, TException> context,
        IBehavior<LifeStreamMachineState, ModifyingLifeStreamRequested> next)
        where TException : Exception
    {
        return next.Faulted(context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("modifying-lifeStream");
    }
}
