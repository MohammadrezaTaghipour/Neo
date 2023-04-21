using EventStore.Client;
using Neo.Infrastructure.Framework.Domain;
using Neo.Infrastructure.Framework.Persistence;
using Newtonsoft.Json;
using System.Text;

namespace Neo.Infrastructure.EventStore;

public class EsdbRepository<TAggregate, TState, TId> :
    IEventSourcedRepository<TAggregate, TState, TId>
    where TAggregate : EventSourcedAggregate<TState>
    where TState : AggregateState<TState>, new()
    where TId : AggregateId
{
    private readonly EventStoreClient _client;
    private readonly IEventReader _eventReader;
    private readonly IDomainEventFactory _domainEventFactory;


    public EsdbRepository(EventStoreClient client,
        IEventReader eventReader,
        IDomainEventFactory domainEventFactory)
    {
        _client = client;
        _eventReader = eventReader;
        _domainEventFactory = domainEventFactory;
    }

    public async Task Add(StreamName streamName, TAggregate aggregate,
        CancellationToken cancellationToken)
    {
        if (aggregate.Changes.Count == 0)
            return;
        var expectedVersion = aggregate.OriginalVersion;
        var proposedEvents = Create(aggregate);
        switch (expectedVersion)
        {
            case (long)ExpectedStreamVersion.NoStream:
                await _client.AppendToStreamAsync(
                    streamName,
                    StreamState.NoStream,
                    proposedEvents,
                    cancellationToken: cancellationToken
                ).ConfigureAwait(false);
                break;
            default:
                await _client.AppendToStreamAsync(
                    streamName,
                    StreamRevision.FromInt64(expectedVersion),
                    proposedEvents,
                    cancellationToken: cancellationToken
                ).ConfigureAwait(false);
                break;
        }
    }

    public async Task<TAggregate> GetById(StreamName streamName, TId id,
        CancellationToken cancellationToken)
    {
        var resolvedEvents = await _eventReader.ReadEvents(
            streamName, 4096, cancellationToken)
            .ConfigureAwait(false);
        var domainEvents = _domainEventFactory.Create(resolvedEvents);
        return AggregateFactory.Create<TAggregate, TState>(domainEvents);
    }

    public async Task<TAggregate> GetBy(StreamName streamName, TId id,
        long version, CancellationToken cancellationToken)
    {
        var aggregate = await GetById(streamName, id, cancellationToken)
            .ConfigureAwait(false);
        if (HasVersionChanged(aggregate, version))
            throw new Exception($"Invalid version exception.");
        return aggregate;
    }

    static bool HasVersionChanged(TAggregate aggregate, long version)
        => aggregate.Changes.Count + version != aggregate.CurrentVersion;

    static IReadOnlyCollection<EventData> Create(TAggregate aggregate)
    {
        var eventData = new List<EventData>();
        aggregate.Changes.OrderBy(a => a.PublishedOn)
            .ToList()
            .ForEach(e => { eventData.Add(CreateEventData(e, null)); });
        return eventData;

        EventData CreateEventData(IDomainEvent @event, byte[] metadata)
        {
            var type = @event.GetType().Name;
            var serialized = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event));
            return new EventData(Uuid.FromGuid(@event.EventId), type, serialized, metadata);
        }
    }
}