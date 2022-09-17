using System.Text;
using EventStore.Client;
using Neo.Infrastructure.Framework.Domain;
using Neo.Infrastructure.Framework.Persistence;
using Newtonsoft.Json;

namespace Neo.Infrastructure.EventStore;

public class EsdbRepository<TAggregate, TState, TId> :
    IEventSourcedRepository<TAggregate, TState, TId>
    where TAggregate : EventSourcedAggregate<TState>
    where TState : AggregateState<TState>, new()
    where TId : AggregateId
{
    private readonly EventStoreClient _client;
    private readonly IDomainEventFactory _domainEventFactory;

    public EsdbRepository(EventStoreClient client,
        IDomainEventFactory domainEventFactory)
    {
        _client = client;
        _domainEventFactory = domainEventFactory;
    }

    public async Task Add(StreamName streamName, TAggregate aggregate,
        CancellationToken cancellationToken)
    {
        if (aggregate.Changes.Count == 0) return;
        var expectedVersion = aggregate.OriginalVersion;
        var proposedEvents = Create(aggregate);
        switch (expectedVersion)
        {
            case (int)ExpectedStreamVersion.NoStream:
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
        var stream = _client.ReadStreamAsync(Direction.Forwards,
            streamName,
            StreamPosition.Start,
            4096,
            cancellationToken: cancellationToken);

        if (await stream.ReadState != ReadState.Ok)
            throw new Exception($"{stream.ReadState}");

        var resolvedEvents = await stream
            .ToArrayAsync(cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var domainEvents = _domainEventFactory.Create(resolvedEvents);

        return AggregateFactory.Create<TAggregate, TState, TId>(domainEvents);
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
        => aggregate.Changes.Count + version != aggregate.Version;

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