using MassTransit;
using MassTransit.Courier.Contracts;
using Microsoft.Extensions.Options;
using Neo.Application.Contracts.ReferentialPointers;
using Neo.Application.Contracts.StreamContexts;
using Neo.Application.ReferentialPointers;

namespace Neo.Application.StreamContexts.Activities;

public class OnDefiningStreamContextRequested :
    IStateMachineActivity<StreamContextMachineState, DefiningStreamContextRequested>
{
    private readonly MassTransitOptions _options;

    public OnDefiningStreamContextRequested(IOptions<MassTransitOptions> options)
    {
        _options = options.Value;
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<StreamContextMachineState,
        DefiningStreamContextRequested> context,
        IBehavior<StreamContextMachineState, DefiningStreamContextRequested> next)
    {
        context.Saga.StreamContextId = context.Message.Id;

        UpdateReferentialPointersState(context.Saga, context.Message);

        var builder = new RoutingSlipBuilder(NewId.NextGuid());

        builder.AddActivity(
            nameof(DefineStreamContextActivity),
            RoutingSlipAddress.ForQueue<DefineStreamContextActivity,
                DefiningStreamContextRequested>(),
            context.Message);

        builder.AddActivity(
            nameof(SyncReferentialPointersActivity),
            RoutingSlipAddress.ForQueue<SyncReferentialPointersActivity,
                SyncingReferentialPointersRequested>(),
            new SyncingReferentialPointersRequested
            {
                Id = context.Message.Id,
                CurrentState = context.Saga.ReferentialPointerCurrentState,
                NextState = context.Saga.ReferentialPointerNextState
            });

        builder.AddActivity(
            nameof(SyncStreamContextProjectionActivity),
            RoutingSlipAddress.ForQueue<SyncStreamContextProjectionActivity,
                SyncStreamContextProjection>(),
            new SyncStreamContextProjection
            {
                Id = context.Message.Id,
            });

        await builder.AddSubscription(
            new Uri(_options.StreamContextStateMachineAddress),
                RoutingSlipEvents.Completed | RoutingSlipEvents.Supplemental,
                RoutingSlipEventContents.Data,
                x => x.Send(new StreamContextActivitiesCompleted
                {
                    RequestId = context.Message.RequestId,
                    Id = context.Message.Id
                }));

        var routingSlip = builder.Build();
        await context.Execute(routingSlip);

        await next.Execute(context).ConfigureAwait(false);
    }

    public Task Faulted<TException>(BehaviorExceptionContext<StreamContextMachineState,
        DefiningStreamContextRequested, TException> context,
        IBehavior<StreamContextMachineState, DefiningStreamContextRequested> next)
        where TException : Exception
    {
        return next.Faulted(context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("defining-streamContext");
    }

    private static void UpdateReferentialPointersState(
        StreamContextMachineState machineState,
        DefiningStreamContextRequested request)
    {
        machineState.ReferentialPointerNextState.DefinedItems
            .Add(new ReferentialStateRecord(request.Id,
                ReferentialPointerType.StreamContext.ToString()));

        foreach (var item in request.StreamEventTypes)
        {
            machineState.ReferentialPointerNextState.UsedItems
                .Add(new ReferentialStateRecord(item.StreamEventTypeId,
                    ReferentialPointerType.StreamContext.ToString()));
        }
    }
}
