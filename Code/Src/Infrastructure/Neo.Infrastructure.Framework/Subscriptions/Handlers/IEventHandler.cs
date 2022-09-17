using Neo.Infrastructure.Framework.Subscriptions.Contexts;

namespace Neo.Infrastructure.Framework.Subscriptions.Handlers;

public interface IEventHandler
{
    Task HandleEvent(IMessageConsumeContext context);
}