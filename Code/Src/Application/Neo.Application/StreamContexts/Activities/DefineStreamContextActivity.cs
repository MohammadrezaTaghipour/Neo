using MassTransit;
using Neo.Application.Contracts;
using Neo.Application.Contracts.StreamContexts;
using Neo.Infrastructure.Framework.Application;
using Neo.Infrastructure.Framework.Domain;

namespace Neo.Application.StreamContexts.Activities;

public class DefineStreamContextActivity :
    IActivity<DefiningStreamContextRequested, StreamContextActivityLog>
{
    private readonly ICommandBus _commandBus;

    public DefineStreamContextActivity(ICommandBus commandBus)
    {
        _commandBus = commandBus;
    }

    public async Task<ExecutionResult> Execute(
        ExecuteContext<DefiningStreamContextRequested> context)
    {
        try
        {
            var request = context.Arguments;

            await _commandBus.Dispatch(request, context.CancellationToken)
                .ConfigureAwait(false);

            await context.Send(context.SourceAddress,
                new DefiningStreamContextRequestExecuted
                {
                    Id = request.Id
                });

            return context.Completed(
                new StreamContextActivityLog
                {
                    StreamContextId = request.Id
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
        CompensateContext<StreamContextActivityLog> context)
    {
        await _commandBus.Dispatch(new RemovingStreamContextRequested
        {
            Id = context.Log.StreamContextId
        }, context.CancellationToken)
            .ConfigureAwait(false);

        return context.Compensated();
    }
}
