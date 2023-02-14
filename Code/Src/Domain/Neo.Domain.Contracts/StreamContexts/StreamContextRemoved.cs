using Neo.Infrastructure.Framework.Domain;

namespace Neo.Domain.Contracts.StreamContexts;

public class StreamContextRemoved : DomainEvent
{
    public StreamContextRemoved(StreamContextId id)
    {
        Id = id;
    }

    public StreamContextId Id { get; }
}