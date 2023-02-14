using Neo.Infrastructure.Framework.Domain;

namespace Neo.Infrastructure.Framework.Persistence;

public interface IDomainEventFactory
{
    IReadOnlyList<DomainEvent> Create(StreamEvent[] events);
}