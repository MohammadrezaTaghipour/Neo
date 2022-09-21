using Neo.Infrastructure.EventStore.Serializations;
using Neo.Infrastructure.Framework.Domain;
using Neo.Infrastructure.Framework.Persistence;

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

    public IReadOnlyList<DomainEvent> Create(StreamEvent[] events)
    {
        return events.Select(e =>
        {
            //TODO: ?? e.ContentType
            var type = _mapper.GetType(e.ContentType);
            return Create(e, type, _serializer);
        }).ToList();
    }

    static DomainEvent Create(StreamEvent @event,
        Type eventType, IEventSerializer serializer)
    {
        var instance = serializer.Deserialize(@event.Payload.ToString(), eventType);
        return instance as DomainEvent;
    }
}