using MassTransit;
using Neo.Application.Contracts.ReferentialPointers;
using Neo.Domain.Contracts.ReferentialPointers;
using Neo.Infrastructure.Framework.Application;

namespace Neo.Application.ReferentialPointers;

public class SyncReferentialPointersActivity :
    IExecuteActivity<SyncingReferentialPointersRequested>
{
    private readonly ICommandBus _commandBus;

    public SyncReferentialPointersActivity(ICommandBus commandBus)
    {
        _commandBus = commandBus;
    }

    public async Task<ExecutionResult> Execute(
        ExecuteContext<SyncingReferentialPointersRequested> context)
    {
        if (context.Arguments.NextState == null)
            return context.Completed();

        var nextState = context.Arguments.NextState;
        nextState.DefinedItems.ToList().ForEach(async a =>
        {
            await _commandBus.Dispatch(
                new ReferentialPointerDefined(
                 new ReferentialPointerId(a.Id),
                 a.ReferentialType), context.CancellationToken)
            .ConfigureAwait(false);
        });

        nextState.UsedItems.ToList().ForEach(async a =>
        {
            await _commandBus.Dispatch(
                new ReferentialPointerMarkedAsUsed(
                new ReferentialPointerId(a.Id),
                a.ReferentialType), context.CancellationToken)
            .ConfigureAwait(false);
        });

        nextState.UnusedItems.ToList().ForEach(async a =>
        {
            await _commandBus.Dispatch(
                new ReferentialPointerMarkedAsUnused(
                new ReferentialPointerId(a.Id),
                a.ReferentialType), context.CancellationToken)
            .ConfigureAwait(false);
        });

        nextState.RemovedItems.ToList().ForEach(async a =>
        {
            await _commandBus.Dispatch(
                new ReferentialPointerRemoved(
                new ReferentialPointerId(a.Id),
                a.ReferentialType), context.CancellationToken)
            .ConfigureAwait(false);
        });

        await context.Send(context.SourceAddress,
            new ReferentialPointersSynced
            {
                Id = context.Arguments.Id
            }).ConfigureAwait(false);

        return context.Completed();
    }
}
