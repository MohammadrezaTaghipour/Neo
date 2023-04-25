using MassTransit;
using Neo.Application.Contracts;
using Neo.Application.Contracts.StreamContexts;
using Neo.Infrastructure.Framework.Application;
using Neo.Infrastructure.Framework.Domain;

namespace Neo.Application.StreamContexts.Activities;

public class ModifyStreamContextActivity :
    IActivity<ModifyingStreamContextRequested, StreamContextActivityLog>
{
    private readonly ICommandBus _commandBus;

    public ModifyStreamContextActivity(ICommandBus commandBus)
    {
        _commandBus = commandBus;
    }

    public async Task<ExecutionResult> Execute(
        ExecuteContext<ModifyingStreamContextRequested> context)
    {
        try
        {
            var request = context.Arguments;

            await _commandBus.Dispatch(request, context.CancellationToken)
                .ConfigureAwait(false);

            await context.Send(context.SourceAddress,
                new ModifyingStreamContextRequestExecuted
                {
                    Id = request.Id
                }).ConfigureAwait(false);

            return context.Completed(
                new StreamContextActivityLog
                {
                    StreamContextId = request.Id
                });
        }
        catch (Exception e)
        {
            await context.Send(context.SourceAddress,
                new ActivitiesFaulted
                {
                    RequestId = context.Arguments.RequestId,
                    Id = context.Arguments.Id,
                    ErrorCode = (e as BusinessException)?.ErrorCode,
                    ErrorMessage = e.Message
                }).ConfigureAwait(false);
            throw;
        }
    }

    public async Task<CompensationResult> Compensate(
        CompensateContext<StreamContextActivityLog> context)
    {
        //TODO: 
        await Task.CompletedTask;
        return context.Compensated();
    }
}
