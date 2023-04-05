using MassTransit;
using Neo.Application.Contracts.LifeStreams;
using Neo.Domain.Contracts.LifeStreams;
using Neo.Domain.Models.LifeStreams;
using Neo.Infrastructure.Framework.Application;
using Neo.Infrastructure.Framework.Persistence;
using Neo.Infrastructure.Projection.MongoDB;

namespace Neo.Application.Query.LifeStreams;

public interface ILifeStreamQueryService : IQueryService
{
    Task<LifeStreamResponse?> Get(Guid id, CancellationToken cancellationToken);
}

public class LifeStreamQueryService : ILifeStreamQueryService
{
    private readonly IAggregateReader _aggregateReader;
    private readonly IRequestClient<LifeStreamStatusRequested> _statusRequestClient;

    public LifeStreamQueryService(
        IAggregateReader aggregateReader,
        IRequestClient<LifeStreamStatusRequested> statusRequestClient)
    {
        _aggregateReader = aggregateReader;
        _statusRequestClient = statusRequestClient;
    }

    public async Task<LifeStreamResponse?> Get(Guid id,
        CancellationToken cancellationToken)
    {
        var streamContextStatus = (await _statusRequestClient
            .GetResponse<LifeStreamStatusRequestExecuted>(
            new LifeStreamStatusRequested
            {
                Id = id
            }).ConfigureAwait(false)).Message;
        if (streamContextStatus.Faulted)
            return LifeStreamResponse.CreateFaulted(
                new StatusResponse(streamContextStatus.Completed,
                streamContextStatus.ErrorCode,
                streamContextStatus.ErrorMessage));

        var lifeStream = await _aggregateReader
            .Load<LifeStream, LifeStreamState>(
                GetStreamName(new LifeStreamId(id)), cancellationToken)
            .ConfigureAwait(false);

        if (lifeStream == null)
            return null;

        return new LifeStreamResponse(lifeStream.State.Id.Value,
            lifeStream.State.Title, lifeStream.State.Description,
            lifeStream.State.Removed,
            lifeStream.State.StreamEvents
                .Select(_ => new LifeStreamEventResponse(_.Id.Value,
                    _.lifeStreamId.Value,
                    _.StreamContextId.Value,
                    _.streamEventTypeId.Value,
                    _.Metadata.Select(meta => new LifeStreamEventMetadaResponse(
                        meta.Key, meta.Value))
                    .ToList()))
                .ToList(),
            new StatusResponse(streamContextStatus.Completed,
                streamContextStatus.ErrorCode,
                streamContextStatus.ErrorMessage));
    }

    static StreamName GetStreamName(LifeStreamId id) =>
        StreamName.For<LifeStream, LifeStreamState, LifeStreamId>(id);
}
