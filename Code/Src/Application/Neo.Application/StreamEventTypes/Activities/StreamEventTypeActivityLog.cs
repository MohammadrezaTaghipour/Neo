using MassTransit;
using Neo.Application.Contracts;
using Neo.Infrastructure.Framework.AspCore;

namespace Neo.Application.StreamEventTypes.Activities;

public class StreamEventTypeActivityLog
{
    public Guid StreamEventTypeId { get; set; }
}


public class OnStreamEventTypeActivitiesFaulted :
    IStateMachineActivity<StreamEventTypeMachineState, ActivitiesFaulted>
{
    private readonly IErrorResponseBuilder _errorResponseBuilder;

    public OnStreamEventTypeActivitiesFaulted(IErrorResponseBuilder errorResponseBuilder)
    {
        _errorResponseBuilder = errorResponseBuilder;
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<StreamEventTypeMachineState,
        ActivitiesFaulted> context, IBehavior<StreamEventTypeMachineState,
            ActivitiesFaulted> next)
    {
        var errorResponse = _errorResponseBuilder
            .Buid(context.Message.ErrorCode, context.Message.ErrorMessage);
        context.Saga.Error = errorResponse;

        await next.Execute(context).ConfigureAwait(false);
    }

    public Task Faulted<TException>(BehaviorExceptionContext<StreamEventTypeMachineState,
        ActivitiesFaulted, TException> context,
        IBehavior<StreamEventTypeMachineState, ActivitiesFaulted> next)
        where TException : Exception
    {
        return next.Faulted(context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("streamEventType-activities-faulted");
    }
}