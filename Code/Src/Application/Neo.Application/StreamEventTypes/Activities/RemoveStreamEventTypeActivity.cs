using MassTransit;
using Neo.Application.Contracts;
using Neo.Application.Contracts.StreamEventTypes;
using Neo.Domain.Contracts.StreamEventTypes;
using Neo.Domain.Models.StreamEventTypes;
using Neo.Infrastructure.Framework.Application;
using Neo.Infrastructure.Framework.Domain;

namespace Neo.Application.StreamEventTypes.Activities;

public class RemoveStreamEventTypeActivity :
    IActivity<RemovingStreamEventTypeRequested, StreamEventTypeActivityLog>
{
    private readonly ICommandBus _commandBus;
    private readonly IStreamEventTypeRepository _repository;

    public RemoveStreamEventTypeActivity(ICommandBus commandBus,
        IStreamEventTypeRepository repository)
    {
        _commandBus = commandBus;
        _repository = repository;
    }

    public async Task<ExecutionResult> Execute(
        ExecuteContext<RemovingStreamEventTypeRequested> context)
    {
        try
        {
            var request = context.Arguments;

            await _commandBus.Dispatch(request, context.CancellationToken)
                .ConfigureAwait(false);

            var streamEventType = (await _repository
                .GetBy(new StreamEventTypeId(request.Id), context.CancellationToken));

            await context.Send(context.SourceAddress,
                new RemovingStreamEventTypeRequestExecuted
                {
                    Id = request.Id,
                    OriginalVersion = streamEventType.OriginalVersion,
                    CurrentVersion = streamEventType.CurrentVersion,
                }).ConfigureAwait(false);

            return context.Completed(
                new StreamEventTypeActivityLog
                {
                    StreamEventTypeId = request.Id
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
                    ErrorMessage = e.Message,
                }).ConfigureAwait(false);
            throw;
        }
    }

    public async Task<CompensationResult> Compensate(
        CompensateContext<StreamEventTypeActivityLog> context)
    {
        await Task.CompletedTask;
        return context.Compensated();
    }
}
