using Neo.Domain.Contracts.LifeStreams;
using Neo.Domain.Models.LifeStreams;
using Neo.Infrastructure.Framework.Application;
using Neo.Infrastructure.Framework.Persistence;

namespace Neo.Application.Query.LifeStreams;

public interface ILifeStreamQueryService : IQueryService
{
    Task<LifeStreamResponse> Get(Guid id, CancellationToken cancellationToken);
}

public class LifeStreamQueryService : ILifeStreamQueryService
{
    private readonly IAggregateReader _aggregateReader;
    
    public LifeStreamQueryService(IAggregateReader aggregateReader)
    {
        _aggregateReader = aggregateReader;
    }

    public async Task<LifeStreamResponse> Get(Guid id,
        CancellationToken cancellationToken)
    {
        var stream = await _aggregateReader
            .Load<LifeStream, LifeStreamState>(
                GetStreamName(new LifeStreamId(id)), cancellationToken)
            .ConfigureAwait(false);

        return new LifeStreamResponse(stream.State.Id.Value,
            stream.State.Title, stream.State.Description,
            stream.State.Removed,
            stream.State.StreamEvents.Select(_ => new LifeStreamEventResponse(_.Id.Value,
                _.lifeStreamId.Value, _.StreamContextId.Value,
                _.streamEventTypeId.Value,
                _.Metadata.Select(meta => new LifeStreamEventMetadaResponse(meta.Key, meta.Value))
                .ToList()))
            .ToList());
    }

    static StreamName GetStreamName(LifeStreamId id) =>
        StreamName.For<LifeStream, LifeStreamState, LifeStreamId>(id);
}
