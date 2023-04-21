using Neo.Infrastructure.Framework.Subscriptions.Contexts;

namespace Neo.Infrastructure.Framework.Subscriptions;

public record EventPosition(ulong? Position, DateTime Created)
{
    public static EventPosition FromContext(IMessageConsumeContext context)
        => new(context.StreamPosition, context.Created);
}