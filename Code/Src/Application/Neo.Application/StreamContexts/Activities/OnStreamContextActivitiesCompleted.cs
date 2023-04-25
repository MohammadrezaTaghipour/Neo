using MassTransit;
using Neo.Application.Contracts.StreamContexts;
using Neo.Infrastructure.Framework.Notifications;

namespace Neo.Application.StreamContexts.Activities;

public class OnStreamContextActivitiesCompleted :
    IStateMachineActivity<StreamContextMachineState, StreamContextActivitiesCompleted>
{
    private readonly INotificationPublisher _notificationPublisher;

    public OnStreamContextActivitiesCompleted(
        INotificationPublisher notificationPublisher)
    {
        _notificationPublisher = notificationPublisher;
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(
        BehaviorContext<StreamContextMachineState,
            StreamContextActivitiesCompleted> context,
        IBehavior<StreamContextMachineState,
            StreamContextActivitiesCompleted> next)
    {
        await _notificationPublisher
            .Publish(RequestStatusNotificationMessage.Success(
                context.Message.RequestId, context.Message.Id,
                context.Saga.CurrentState))
            .ConfigureAwait(false);

        await next.Execute(context).ConfigureAwait(false);
    }

    public Task Faulted<TException>(
        BehaviorExceptionContext<StreamContextMachineState,
            StreamContextActivitiesCompleted, TException> context,
        IBehavior<StreamContextMachineState, StreamContextActivitiesCompleted> next)
        where TException : Exception
    {
        return next.Faulted(context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("streamContext-activities-completed");
    }
}