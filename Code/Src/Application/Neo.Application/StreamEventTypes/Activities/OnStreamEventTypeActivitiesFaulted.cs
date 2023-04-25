using MassTransit;
using Neo.Application.Contracts;
using Neo.Infrastructure.Framework.AspCore;
using Neo.Infrastructure.Framework.Notifications;

namespace Neo.Application.StreamEventTypes.Activities;

public class OnStreamEventTypeActivitiesFaulted :
    IStateMachineActivity<StreamEventTypeMachineState, ActivitiesFaulted>
{
    private readonly IErrorResponseBuilder _errorResponseBuilder;
    private readonly INotificationPublisher _notificationPublisher;

    public OnStreamEventTypeActivitiesFaulted(
        IErrorResponseBuilder errorResponseBuilder,
        INotificationPublisher notificationPublisher)
    {
        _errorResponseBuilder = errorResponseBuilder;
        _notificationPublisher = notificationPublisher;
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(
        BehaviorContext<StreamEventTypeMachineState, ActivitiesFaulted> context,
        IBehavior<StreamEventTypeMachineState, ActivitiesFaulted> next)
    {
        var errorResponse = _errorResponseBuilder
            .Buid(context.Message.ErrorCode, context.Message.ErrorMessage);

        await _notificationPublisher
            .Publish(RequestStatusNotificationMessage.Failed(
                context.Message.RequestId, context.Message.Id,
                context.Saga.CurrentState,
                errorResponse.Code, errorResponse.Message))
            .ConfigureAwait(false);

        await next.Execute(context).ConfigureAwait(false);
    }

    public Task Faulted<TException>(
        BehaviorExceptionContext<StreamEventTypeMachineState,
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
