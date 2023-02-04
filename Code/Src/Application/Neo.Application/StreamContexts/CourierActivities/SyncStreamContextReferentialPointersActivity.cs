using MassTransit;
using Neo.Application.Contracts.StreamContexts;

namespace Neo.Application.StreamContexts.CourierActivities;

public class SyncStreamContextReferentialPointersActivity :
    IExecuteActivity<SyncingStreamContextReferentialPointersRequest>
{
    public Task<ExecutionResult> Execute(
        ExecuteContext<SyncingStreamContextReferentialPointersRequest> context)
    {
        throw new NotImplementedException();
    }
}
