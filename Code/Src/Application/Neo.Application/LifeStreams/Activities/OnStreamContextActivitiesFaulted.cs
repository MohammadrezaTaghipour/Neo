using MassTransit;
using Neo.Application.Contracts;
using Neo.Infrastructure.Framework.AspCore;

namespace Neo.Application.LifeStreams.Activities;

public class OnLifeStreamActivitiesFaulted :
    IStateMachineActivity<LifeStreamMachineState, ActivitiesFaulted>
{
    private readonly IErrorResponseBuilder _errorResponseBuilder;

    public OnLifeStreamActivitiesFaulted(IErrorResponseBuilder errorResponseBuilder)
    {
        _errorResponseBuilder = errorResponseBuilder;
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<LifeStreamMachineState,
        ActivitiesFaulted> context, IBehavior<LifeStreamMachineState,
            ActivitiesFaulted> next)
    {
        var errorResponse = _errorResponseBuilder
            .Buid(context.Message.ErrorCode,
            context.Message.ErrorMessage);
        context.Saga.Error = new ErrorResponse(
            errorResponse.Code, errorResponse.Message);

        await next.Execute(context).ConfigureAwait(false);
    }

    public Task Faulted<TException>(BehaviorExceptionContext<LifeStreamMachineState,
        ActivitiesFaulted, TException> context,
        IBehavior<LifeStreamMachineState, ActivitiesFaulted> next)
        where TException : Exception
    {
        return next.Faulted(context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("lifeStream-activities-faulted");
    }
}