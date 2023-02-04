using MassTransit;
using Neo.Application.Contracts.StreamContexts;
using Neo.Application.StreamContexts.CourierActivities;

namespace Neo.Application.StreamContexts.StateMachineActivities;

public class OnDefiningStreamContextRequested :
    IStateMachineActivity<StreamContextState, DefiningStreamContextRequested>
{
    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<StreamContextState,
        DefiningStreamContextRequested> context,
        IBehavior<StreamContextState, DefiningStreamContextRequested> next)
    {
        var builder = new RoutingSlipBuilder(NewId.NextGuid());

        builder.AddActivity(nameof(DefineStreamContextActivity),
            RoutingSlipAddress.ForQueue<DefineStreamContextActivity,
                DefiningStreamContextRequested>(),
            context.Message);

        builder.AddActivity(nameof(SyncStreamContextReferentialPointersActivity),
            RoutingSlipAddress.ForQueue<SyncStreamContextReferentialPointersActivity,
                SyncingStreamContextReferentialPointersRequest>(),
            new SyncingStreamContextReferentialPointersRequest
            {
                Id = context.Message.Id
            });

        var routingSlip = builder.Build();
        await context.Execute(routingSlip);

        await next.Execute(context).ConfigureAwait(false);
    }

    public Task Faulted<TException>(BehaviorExceptionContext<StreamContextState,
        DefiningStreamContextRequested, TException> context,
        IBehavior<StreamContextState, DefiningStreamContextRequested> next)
        where TException : Exception
    {
        return next.Faulted(context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("defining-streamContext");
    }
}
