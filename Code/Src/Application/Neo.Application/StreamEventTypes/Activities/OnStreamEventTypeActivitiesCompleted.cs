using MassTransit;
using Neo.Application.Contracts.StreamEventTypes;
using Neo.Infrastructure.Framework.Notifications;

namespace Neo.Application.StreamEventTypes.Activities;

public class OnStreamEventTypeActivitiesCompleted :
    IStateMachineActivity<StreamEventTypeMachineState, StreamEventTypeActivitiesCompleted>
{
    private readonly INotificationPublisher _notificationPublisher;

    public OnStreamEventTypeActivitiesCompleted(
        INotificationPublisher notificationPublisher)
    {
        _notificationPublisher = notificationPublisher;
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(
        BehaviorContext<StreamEventTypeMachineState,
            StreamEventTypeActivitiesCompleted> context,
        IBehavior<StreamEventTypeMachineState,
            StreamEventTypeActivitiesCompleted> next)
    {
        await _notificationPublisher
            .Publish(RequestStatusNotificationMessage.Success(
                context.Message.RequestId, context.Message.Id,
                context.Saga.CurrentState))
            .ConfigureAwait(false);

        await next.Execute(context).ConfigureAwait(false);
    }

    public Task Faulted<TException>(
        BehaviorExceptionContext<StreamEventTypeMachineState,
            StreamEventTypeActivitiesCompleted, TException> context,
        IBehavior<StreamEventTypeMachineState, StreamEventTypeActivitiesCompleted> next)
        where TException : Exception
    {
        return next.Faulted(context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("streamEventType-activities-completed");
    }
}