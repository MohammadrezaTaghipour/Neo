﻿using MassTransit;
using Neo.Application.Contracts;
using Neo.Application.Contracts.StreamEventTypes;
using Neo.Infrastructure.Framework.Application;
using Neo.Infrastructure.Framework.Domain;

namespace Neo.Application.StreamEventTypes.Activities;

public class DefineStreamEventTypeActivity :
    IActivity<DefiningStreamEventTypeRequested, StreamEventTypeActivityLog>
{
    private readonly ICommandBus _commandBus;

    public DefineStreamEventTypeActivity(ICommandBus commandBus)
    {
        _commandBus = commandBus;
    }

    public async Task<ExecutionResult> Execute(
        ExecuteContext<DefiningStreamEventTypeRequested> context)
    {
        try
        {
            var request = context.Arguments;

            await _commandBus.Dispatch(request, context.CancellationToken)
                .ConfigureAwait(false);

            await context.Send(context.SourceAddress,
                new DefiningStreamEventTypeRequestExecuted
                {
                    Id = request.Id
                }).ConfigureAwait(false);

            return context.Completed(
                new StreamEventTypeActivityLog
                {
                    StreamEventTypeId = request.Id
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
        CompensateContext<StreamEventTypeActivityLog> context)
    {
        await _commandBus.Dispatch(new RemovingStreamEventTypeRequested
        {
            Id = context.Log.StreamEventTypeId
        }, context.CancellationToken)
            .ConfigureAwait(false);

        return context.Compensated();
    }
}
