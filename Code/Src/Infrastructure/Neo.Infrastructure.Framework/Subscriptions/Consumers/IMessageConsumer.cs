using Neo.Infrastructure.Framework.Subscriptions.Contexts;

namespace Neo.Infrastructure.Framework.Subscriptions.Consumers;

public interface IMessageConsumer<in TContext> 
    where TContext : class, IMessageConsumeContext {
    Task Consume(TContext context);
}

public interface IMessageConsumer : IMessageConsumer<IMessageConsumeContext> { }