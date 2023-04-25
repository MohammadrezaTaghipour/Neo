using MassTransit;
using MassTransit.Courier.Contracts;
using Microsoft.Extensions.Options;
using Neo.Application.Contracts.StreamEventTypes;
using Neo.Infrastructure.Framework.Notifications;

namespace Neo.Application.StreamEventTypes.Activities;

public class OnModifyingStreamEventTypeRequested :
    IStateMachineActivity<StreamEventTypeMachineState,
        ModifyingStreamEventTypeRequested>
{
    private readonly MassTransitOptions _options;

    public OnModifyingStreamEventTypeRequested(
        IOptions<MassTransitOptions> options)
    {
        _options = options.Value;
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(
        BehaviorContext<StreamEventTypeMachineState,
            ModifyingStreamEventTypeRequested> context,
        IBehavior<StreamEventTypeMachineState,
            ModifyingStreamEventTypeRequested> next)
    {
        var builder = new RoutingSlipBuilder(NewId.NextGuid());

        builder.AddActivity(nameof(ModifyStreamEventTypeActivity),
            RoutingSlipAddress.ForQueue<ModifyStreamEventTypeActivity,
                ModifyingStreamEventTypeRequested>(),
            context.Message);

        builder.AddActivity(
            nameof(SyncStreamEventTypeProjectionActivity),
            RoutingSlipAddress.ForQueue<SyncStreamEventTypeProjectionActivity,
                SyncStreamEventTypeProjection>(),
            new SyncStreamEventTypeProjection
            {
                Id = context.Message.Id,
            });

        await builder.AddSubscription(
            new Uri(_options.StreamEventTypeStateMachineAddress),
            RoutingSlipEvents.Completed,
             RoutingSlipEventContents.Data,
             x => x.Send(new StreamEventTypeActivitiesCompleted
             {
                 Id = context.Message.Id,
                 RequestId = context.Message.RequestId
             }));

        var routingSlip = builder.Build();
        await context.Execute(routingSlip);

        await next.Execute(context).ConfigureAwait(false);
    }

    public Task Faulted<TException>(
        BehaviorExceptionContext<StreamEventTypeMachineState,
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
