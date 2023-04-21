using MassTransit;
using Neo.Application.Contracts;
using Neo.Application.Contracts.LifeStreams;
using Neo.Infrastructure.Framework.Application;
using Neo.Infrastructure.Framework.Domain;

namespace Neo.Application.LifeStreams.Activities;

public class AppendStreamEventActivity :
    IActivity<AppendngStreamEventRequested, LifeStreamActivityLog>
{
    private readonly ICommandBus _commandBus;

    public AppendStreamEventActivity(ICommandBus commandBus)
    {
        _commandBus = commandBus;
    }

    public async Task<ExecutionResult> Execute(
        ExecuteContext<AppendngStreamEventRequested> context)
    {
        try
        {
            var request = context.Arguments;

            await _commandBus
                .Dispatch(request, context.CancellationToken)
                .ConfigureAwait(false);

            await context.Send(context.SourceAddress,
                new PartialModifyingLifeStreamRequestExecuted
                {
                    Id = request.LifeStreamId
                }).ConfigureAwait(false);

            return context.Completed(
                new LifeStreamActivityLog
                {
                    LifeStreamId = request.LifeStreamId
                });
        }
        catch (Exception e)
        {
            await context.Send(context.SourceAddress,
                new ActivitiesFaulted
                {
                    Id = context.Arguments.LifeStreamId,
                    ErrorCode = (e as BusinessException)?.ErrorCode,
                    ErrorMessage = e.Message
                }).ConfigureAwait(false);
            throw;
        }
    }

    public async Task<CompensationResult> Compensate(
        CompensateContext<LifeStreamActivityLog> context)
    {
        await Task.CompletedTask;
        return context.Compensated();
    }
}
