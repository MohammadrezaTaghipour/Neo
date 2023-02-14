using EventStore.Client;
using Neo.Infrastructure.EventStore.Serializations;
using Neo.Infrastructure.EventStore.Subscriptions;
using Neo.Infrastructure.Framework.Persistence;
using System.Text;

namespace Neo.Infrastructure.EventStore;

public class EsDbEventReader : IEventReader
{
    private readonly EventStoreClient _client;
    private readonly IEventSerializer _eventSerializer;
    private readonly DomainEventTypeMapper _mapper;

    public EsDbEventReader(
        EventStoreClient client,
        IEventSerializer eventSerializer,
        DomainEventTypeMapper mapper)
    {
        _client = client;
        _eventSerializer = eventSerializer;
        _mapper = mapper;
    }

    public async Task<StreamEvent[]> ReadEvents(
        StreamName streamName,
        int count,
        CancellationToken cancellationToken)
    {
        if (!await StreamExists(streamName, cancellationToken))
            return null;

        var stream = _client.ReadStreamAsync(
            Direction.Forwards,
            streamName,
            StreamPosition.Start,
            count,
            cancellationToken: cancellationToken);

        try
        {
            var resolvedEvents = await stream
                .ToArrayAsync(cancellationToken)
                .ConfigureAwait(false);

            return ToStreamEvents(resolvedEvents);
        }
        catch (Exception e)
        {
            throw new ReadFromStreamException(e.Message, e);
        }
    }

    async Task<bool> StreamExists(StreamName stream, CancellationToken cancellationToken)
    {
        var read = _client.ReadStreamAsync(
            Direction.Backwards,
            stream,
            StreamPosition.End,
            1,
            cancellationToken: cancellationToken
        );

        var state = await read.ReadState.ConfigureAwait(false);
        return state == ReadState.Ok;
    }

    StreamEvent[] ToStreamEvents(ResolvedEvent[] resolvedEvents)
        => resolvedEvents.Where(x => !x.Event.EventType.StartsWith("$"))
        .Select(ToStreamEvent).ToArray();

    StreamEvent ToStreamEvent(ResolvedEvent resolvedEvent)
    {
        var payload = DeserializeData(
            resolvedEvent.Event.EventType,
            resolvedEvent.Event.Data,
            resolvedEvent.OriginalStreamId,
            resolvedEvent.Event.Position.CommitPosition
        );

        return new StreamEvent(
                resolvedEvent.Event.EventId.ToGuid(),
                payload,
                MetadataSerializer.Deserialize(
                    resolvedEvent.Event.Metadata,
                    resolvedEvent.OriginalStreamId,
                    resolvedEvent.Event.EventNumber),
                resolvedEvent.Event.EventType,
                resolvedEvent.OriginalEventNumber.ToInt64()
            );
    }

    private object DeserializeData(
      string eventType,
      ReadOnlyMemory<byte> data,
      string stream,
      ulong position = 0)
    {
        try
        {
            var type = _mapper.GetType(eventType);
            var dataStr = Encoding.UTF8.GetString(data.Span);
            var result = _eventSerializer.Deserialize(dataStr, type);
            return result;
        }
        catch (Exception e)
        {
            throw new DeserializationException(stream, eventType, position, e);
        }
    }
}
