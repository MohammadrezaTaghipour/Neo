using Neo.Infrastructure.Framework.Subscriptions.Contexts;

namespace Neo.Infrastructure.Framework.Persistence;

public record StreamEvent(Guid Id, object Payload, Metadata Metadata, string ContentType, long Position);