using Neo.Infrastructure.Framework.Domain;
using Neo.Infrastructure.Framework.Persistence;

namespace Neo.Infrastructure.EventStore;

public class DomainEventFactory : IDomainEventFactory
{
    public IReadOnlyList<DomainEvent> Create(StreamEvent[] events)
    {
        return events.Select(e =>
        {
            return Create(e);
        }).ToList();
    }

    static DomainEvent Create(StreamEvent @event)
    {
        return @event.Payload as DomainEvent;
    }
}