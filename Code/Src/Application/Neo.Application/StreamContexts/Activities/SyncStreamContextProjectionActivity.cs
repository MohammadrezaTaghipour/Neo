using MassTransit;
using Neo.Application.Contracts.StreamContexts;
using Neo.Application.StreamEventTypes.Activities;

namespace Neo.Application.StreamContexts.Activities;

public class SyncStreamContextProjectionActivity :
    IActivity<SyncStreamContextProjection, StreamContextActivityLog>
{
    public async Task<ExecutionResult> Execute(
        ExecuteContext<SyncStreamContextProjection> context)
    {
        await Task.CompletedTask;
        return context.Completed();
    }

    public async Task<CompensationResult> Compensate(
        CompensateContext<StreamContextActivityLog> context)
    {
        await Task.CompletedTask;
        return context.Compensated();
    }
}