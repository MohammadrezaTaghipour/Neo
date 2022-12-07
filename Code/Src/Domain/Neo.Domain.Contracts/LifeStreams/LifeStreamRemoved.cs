using Neo.Infrastructure.Framework.Domain;

namespace Neo.Domain.Contracts.LifeStreams;

public class LifeStreamRemoved : DomainEvent
{
    public LifeStreamRemoved(LifeStreamId id)
    {
        Id = id;
    }

    public LifeStreamId Id { get; }
}
