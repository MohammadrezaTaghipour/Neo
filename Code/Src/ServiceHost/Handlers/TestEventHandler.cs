using Neo.Infrastructure.Framework.Subscriptions.Contexts;
using Neo.Infrastructure.Framework.Subscriptions.Handlers;

namespace ServiceHost.Handlers;

public class TestEventHandler : IEventHandler
{
    public Task HandleEvent(IMessageConsumeContext context)
    {
        Console.WriteLine($"New event received: {context.MessageType}.");
        return Task.CompletedTask;
    }
}