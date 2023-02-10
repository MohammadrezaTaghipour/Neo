using MassTransit;
using MassTransit.Courier.Contracts;
using Neo.Application.Contracts.ReferentialPointers;
using Neo.Application.Contracts.StreamEventTypes;

namespace Neo.Application.StreamEventTypes.Activities;

public class OnModifyingStreamEventTypeRequested :
    IStateMachineActivity<StreamEventTypeMachineState, ModifyingStreamEventTypeRequested>
{
    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<StreamEventTypeMachineState,
        ModifyingStreamEventTypeRequested> context,
        IBehavior<StreamEventTypeMachineState, ModifyingStreamEventTypeRequested> next)
    {
        var builder = new RoutingSlipBuilder(NewId.NextGuid());

        builder.AddActivity(nameof(ModifyStreamEventTypeActivity),
            RoutingSlipAddress.ForQueue<ModifyStreamEventTypeActivity,
                ModifyingStreamEventTypeRequested>(),
            context.Message);

        var routingSlip = builder.Build();
        await context.Execute(routingSlip);

        await next.Execute(context).ConfigureAwait(false);
    }

    public Task Faulted<TException>(BehaviorExceptionContext<StreamEventTypeMachineState,
        ModifyingStreamEventTypeRequested, TException> context,
        IBehavior<StreamEventTypeMachineState, ModifyingStreamEventTypeRequested> next)
        where TException : Exception
    {
        return next.Faulted(context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("modifying-streamEventType");
    }
}
