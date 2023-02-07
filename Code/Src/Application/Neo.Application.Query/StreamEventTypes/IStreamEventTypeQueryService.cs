using MassTransit;
using Neo.Application.Contracts.StreamEventTypes;
using Neo.Domain.Contracts.StreamEventTypes;
using Neo.Domain.Models.StreamEventTypes;
using Neo.Infrastructure.Framework.Application;
using Neo.Infrastructure.Framework.Persistence;

namespace Neo.Application.Query.StreamEventTypes;

public interface IStreamEventTypeQueryService : IQueryService
{
    Task<StreamEventTypeResponse> Get(Guid id, CancellationToken cancellationToken);
}

public class StreamEventTypeQueryService : IStreamEventTypeQueryService
{
    private readonly IAggregateReader _aggregateReader;
    private readonly IRequestClient<StreamEventTypeStatusRequested> _statusRequestClient;

    public StreamEventTypeQueryService(IAggregateReader aggregateReader,
        IRequestClient<StreamEventTypeStatusRequested> statusRequestClient)
    {
        _aggregateReader = aggregateReader;
        _statusRequestClient = statusRequestClient;
    }

    public async Task<StreamEventTypeResponse> Get(Guid id,
        CancellationToken cancellationToken)
    {
        var streamEventTypeStatus = (await _statusRequestClient
                .GetResponse<StreamEventTypeStatusRequestExecuted>(
                new StreamEventTypeStatusRequested
                {
                    Id = id
                })).Message;
        if (!streamEventTypeStatus.Id.HasValue)
            return new StreamEventTypeResponse(null, null, null,
                null, new StatusResponse(false, streamEventTypeStatus.ErrorCode,
                streamEventTypeStatus.ErrorMessage));

        var streamEventType = await _aggregateReader
            .Load<StreamEventType, StreamEventTypeState>(
                GetStreamName(new StreamEventTypeId(id)), cancellationToken)
            .ConfigureAwait(false);

        return new StreamEventTypeResponse(
            streamEventType.State.Id.Value,
            streamEventType.State.Title,
            streamEventType.State.Removed,
            streamEventType.State.Metadata
                .Select(_ => new StreamEventTypeMetadataResponse(_.Title))
                .ToList(),
            new StatusResponse(streamEventTypeStatus.Completed,
                streamEventTypeStatus.ErrorCode,
                streamEventTypeStatus.ErrorMessage));
    }

    static StreamName GetStreamName(StreamEventTypeId id) =>
        StreamName.For<StreamEventType, StreamEventTypeState, StreamEventTypeId>(id);
}