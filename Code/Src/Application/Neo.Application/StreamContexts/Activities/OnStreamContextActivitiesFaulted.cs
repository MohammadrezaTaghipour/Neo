﻿using MassTransit;
using Neo.Application.Contracts;
using Neo.Infrastructure.Framework.AspCore;

namespace Neo.Application.StreamContexts.Activities;

public class OnStreamContextActivitiesFaulted :
    IStateMachineActivity<StreamContextMachineState, ActivitiesFaulted>
{
    private readonly IErrorResponseBuilder _errorResponseBuilder;

    public OnStreamContextActivitiesFaulted(IErrorResponseBuilder errorResponseBuilder)
    {
        _errorResponseBuilder = errorResponseBuilder;
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<StreamContextMachineState,
        ActivitiesFaulted> context, IBehavior<StreamContextMachineState,
            ActivitiesFaulted> next)
    {
        var errorResponse = _errorResponseBuilder
            .Buid(context.Message.ErrorCode, context.Message.ErrorMessage);
        context.Saga.Error = new ErrorResponse(errorResponse.Code, errorResponse.Message);

        await next.Execute(context).ConfigureAwait(false);
    }

    public Task Faulted<TException>(BehaviorExceptionContext<StreamContextMachineState,
        ActivitiesFaulted, TException> context,
        IBehavior<StreamContextMachineState, ActivitiesFaulted> next)
        where TException : Exception
    {
        return next.Faulted(context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("streamContext-activities-faulted");
    }
}
