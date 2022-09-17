using Neo.Infrastructure.Framework.Subscriptions.Contexts;
using Neo.Infrastructure.Framework.Subscriptions.Handlers;

namespace Neo.Infrastructure.Framework.Subscriptions.Consumers;

public class DefaultMessageConsumer : IMessageConsumer
{
    private readonly IEnumerable<IEventHandler> _eventHandlers;

    public DefaultMessageConsumer(IEnumerable<IEventHandler> eventHandlers)
        => _eventHandlers = eventHandlers;

    public async Task Consume(IMessageConsumeContext context)
    {
        try
        {
            if (context.Message == null) return;

            var tasks = _eventHandlers.Select(a => Handle(a, context));
            await Task.WhenAll(tasks).ConfigureAwait(false);
        }
        catch (Exception)
        {
        }
    }

    async Task Handle(IEventHandler handler, IMessageConsumeContext context)
    {
        await handler.HandleEvent(context).ConfigureAwait(false);
    }
}