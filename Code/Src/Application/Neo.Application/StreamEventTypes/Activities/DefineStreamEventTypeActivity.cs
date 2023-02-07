using MassTransit;
using Neo.Application.Contracts.StreamEventTypes;
using Neo.Infrastructure.Framework.Application;

namespace Neo.Application.StreamEventTypes.Activities;

public class DefineStreamEventTypeActivity :
    IActivity<DefiningStreamEventTypeRequested, DefineStreamEventTypeActivityLog>
{
    private readonly ICommandBus _commandBus;

    public DefineStreamEventTypeActivity(ICommandBus commandBus)
    {
        _commandBus = commandBus;
    }

    public async Task<ExecutionResult> Execute(
        ExecuteContext<DefiningStreamEventTypeRequested> context)
    {
        var ctoken = new CancellationToken();
        var request = context.Arguments;

        await _commandBus.Dispatch(request, ctoken)
            .ConfigureAwait(false);

        await context.Publish(new DefiningStreamEventTypeRequestExecuted
        {
            Id = request.Id
        }).ConfigureAwait(false);

        return context.Completed(
            new DefineStreamEventTypeActivityLog
            {
                StreamEventTypeId = request.Id
            });
    }

    public async Task<CompensationResult> Compensate(
        CompensateContext<DefineStreamEventTypeActivityLog> context)
    {
        await _commandBus.Dispatch(new RemoveStreamEventTypeRequested
        {
            Id = context.Log.StreamEventTypeId
        }, new CancellationToken())
            .ConfigureAwait(false);

        return context.Compensated();
    }
}


public class DefineStreamEventTypeActivityLog
{
    public Guid StreamEventTypeId { get; set; }
}