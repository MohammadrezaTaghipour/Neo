using EventStore.Client;
using Neo.Infrastructure.Framework.Domain;

namespace Neo.Infrastructure.EventStore;

public interface IDomainEventFactory
{
    IReadOnlyList<DomainEvent> Create(ResolvedEvent[] events);
}