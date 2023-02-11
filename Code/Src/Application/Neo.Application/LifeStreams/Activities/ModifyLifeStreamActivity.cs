using MassTransit;
using Neo.Application.Contracts;
using Neo.Application.Contracts.LifeStreams;
using Neo.Infrastructure.Framework.Application;
using Neo.Infrastructure.Framework.Domain;

namespace Neo.Application.LifeStreams.Activities;

public class ModifyLifeStreamActivity :
    IActivity<ModifyingLifeStreamRequested, LifeStreamActivityLog>
{
    private readonly ICommandBus _commandBus;
    public ModifyLifeStreamActivity(ICommandBus commandBus)
    {
        _commandBus = commandBus;
    }

    public async Task<ExecutionResult> Execute(
        ExecuteContext<ModifyingLifeStreamRequested> context)
    {
        try
        {
            var request = context.Arguments;

            await _commandBus.Dispatch(request, context.CancellationToken)
                .ConfigureAwait(false);

            await context.Send(context.SourceAddress,
                new ModifyingLifeStreamRequestExecuted
                {
                    Id = request.Id
                });

            return context.Completed(
                new LifeStreamActivityLog
                {
                    LifeStreamId = request.Id
                });
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            await context.Send(context.SourceAddress,
                new ActivitiesFaulted
                {
                    Id = context.Arguments.Id,
                    ErrorCode = (e as BusinessException)?.ErrorCode,
                    ErrorMessage = e.Message
                });
            throw;
        }
    }

    public async Task<CompensationResult> Compensate(
        CompensateContext<LifeStreamActivityLog> context)
    {
        return context.Compensated();
    }
}
