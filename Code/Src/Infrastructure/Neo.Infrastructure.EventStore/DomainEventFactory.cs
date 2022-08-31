using System.Text;
using EventStore.Client;
using Neo.Infrastructure.EventStore.Serializations;
using Neo.Infrastructure.Framework.Domain;

namespace Neo.Infrastructure.EventStore;

public class DomainEventFactory : IDomainEventFactory
{
    private readonly DomainEventTypeMapper _mapper;
    private readonly IEventSerializer _serializer;

    public DomainEventFactory(DomainEventTypeMapper mapper,
        IEventSerializer serializer)
    {
        _mapper = mapper;
        _serializer = serializer;
    }

    public IReadOnlyList<DomainEvent> Create(ResolvedEvent[] events)
    {
        return events.Select(e =>
        {
            var type = _mapper.GetType(e.Event.EventType);
            return Create(e, type, _serializer);
        }).ToList();
    }

    static DomainEvent Create(ResolvedEvent @event,
        Type eventType, IEventSerializer serializer)
    {
        var data = Encoding.UTF8.GetString(@event.Event.Data.Span);
        var instance = serializer.Deserialize(data, eventType);
        return instance as DomainEvent;
    }
}