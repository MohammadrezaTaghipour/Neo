using MassTransit;
using Neo.Application.Contracts.StreamContexts;
using Neo.Infrastructure.Framework.ReferentialPointers;

namespace Neo.Application.StreamContexts.CourierActivities;

public class SyncReferentialPointersActivity :
    IExecuteActivity<SyncingReferentialPointersRequest>
{
    public async Task<ExecutionResult> Execute(
        ExecuteContext<SyncingReferentialPointersRequest> context)
    {
        if (context.Arguments.NextState == null)
            return context.Completed();

        var nextState = context.Arguments.NextState;
        var message = new List<object>();
        nextState.DefinedItems.ToList().ForEach(a =>
        {
            message.Add(new ReferentialPointerDefined(
                new ReferentialPointerId(a.Id),
                a.ReferentialType));
        });

        nextState.UsedItems.ToList().ForEach(a =>
        {
            message.Add(new ReferentialPointerMarkedAsUsed(
                new ReferentialPointerId(a.Id),
                a.ReferentialType));
        });

        nextState.UnusedItems.ToList().ForEach(a =>
        {
            message.Add(new ReferentialPointerMarkedAsUnused(
                new ReferentialPointerId(a.Id),
                a.ReferentialType));
        });

        nextState.RemovedItems.ToList().ForEach(a =>
        {
            message.Add(new ReferentialPointerRemoved(
                new ReferentialPointerId(a.Id),
                a.ReferentialType));
        });

        await context.PublishBatch(message);

        return context.Completed();
    }
}
