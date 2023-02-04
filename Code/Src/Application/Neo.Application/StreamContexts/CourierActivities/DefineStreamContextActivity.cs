using MassTransit;
using Neo.Application.Contracts.StreamContexts;
using Neo.Infrastructure.Framework.Application;

namespace Neo.Application.StreamContexts.CourierActivities;

public class DefineStreamContextActivity :
    IActivity<DefiningStreamContextRequested, DefineStreamContextActivityLog>
{
    private readonly ICommandBus _commandBus;

    public DefineStreamContextActivity(ICommandBus commandBus)
    {
        _commandBus = commandBus;
    }

    public async Task<ExecutionResult> Execute(
        ExecuteContext<DefiningStreamContextRequested> context)
    {
        var request = context.Arguments;

        await _commandBus.Dispatch(request, new CancellationToken())
            .ConfigureAwait(false);

        await context.Publish(new DefiningStreamContextRequestExecuted
        {
            Id = request.Id
        });

        return context.Completed(
            new DefineStreamContextActivityLog
            {
                StreamContextId = request.Id
            });
    }

    public async Task<CompensationResult> Compensate(
        CompensateContext<DefineStreamContextActivityLog> context)
    {
        await _commandBus.Dispatch(new RemoveStreamContextRequested
        {
            Id = context.Log.StreamContextId
        }, new CancellationToken()).ConfigureAwait(false);

        return context.Compensated();
    }
}

public class DefineStreamContextActivityLog
{
    public Guid StreamContextId { get; set; }
}