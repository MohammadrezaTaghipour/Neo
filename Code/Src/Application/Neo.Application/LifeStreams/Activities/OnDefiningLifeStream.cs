using MassTransit;
using MassTransit.Courier.Contracts;
using Microsoft.Extensions.Options;
using Neo.Application.Contracts.LifeStreams;
using Neo.Application.Contracts.ReferentialPointers;
using Neo.Application.ReferentialPointers;

namespace Neo.Application.LifeStreams.Activities;

public class OnDefiningLifeStream :
    IStateMachineActivity<LifeStreamMachineState,
        DefiningLifeStreamRequested>
{
    private readonly MassTransitOptions _options;

    public OnDefiningLifeStream(IOptions<MassTransitOptions> options)
    {
        _options = options.Value;
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<LifeStreamMachineState,
        DefiningLifeStreamRequested> context,
        IBehavior<LifeStreamMachineState, DefiningLifeStreamRequested> next)
    {
        context.Saga.LifeStreamId = context.Message.Id;

        UpdateReferentialPointersState(context.Saga, context.Message);

        var builder = new RoutingSlipBuilder(NewId.NextGuid());

        builder.AddActivity(nameof(DefineLifeStreamActivity),
            RoutingSlipAddress.ForQueue<DefineLifeStreamActivity,
                DefiningLifeStreamRequested>(),
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
            new Uri(_options.LifeStreamStateMachineAddress),
            RoutingSlipEvents.Completed | RoutingSlipEvents.Supplemental,
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
        DefiningLifeStreamRequested, TException> context,
        IBehavior<LifeStreamMachineState, DefiningLifeStreamRequested> next)
        where TException : Exception
    {
        return next.Faulted(context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("defining-lifeStream");
    }

    private static void UpdateReferentialPointersState(
        LifeStreamMachineState machineState,
        DefiningLifeStreamRequested request)
    {
        machineState.ReferentialPointerNextState = new();
        machineState.ReferentialPointerNextState.DefinedItems
            .Add(new ReferentialStateRecord(request.Id,
                ReferentialPointerType.LifeStream.ToString()));
    }
}
