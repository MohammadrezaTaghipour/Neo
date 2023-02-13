using MassTransit;
using Neo.Application.Contracts.StreamContexts;
using Neo.Domain.Contracts.StreamContexts;
using Neo.Domain.Models.StreamContexts;
using Neo.Infrastructure.Framework.Application;
using Neo.Infrastructure.Framework.Persistence;

namespace Neo.Application.Query.StreamContexts;

public interface IStreamContextQueryService : IQueryService
{
    Task<StreamContextResponse?> Get(Guid id, CancellationToken cancellationToken);
}

public class StreamContextQueryService : IStreamContextQueryService
{
    private readonly IAggregateReader _aggregateReader;
    private readonly IRequestClient<StreamContextStatusRequested> _statusRequestClient;

    public StreamContextQueryService(IAggregateReader aggregateReader,
        IRequestClient<StreamContextStatusRequested> statusRequestClient)
    {
        _aggregateReader = aggregateReader;
        _statusRequestClient = statusRequestClient;
    }

    public async Task<StreamContextResponse?> Get(Guid id,
        CancellationToken cancellationToken)
    {
        var streamContextStatus = (await _statusRequestClient
            .GetResponse<StreamContextStatusRequestExecuted>(
            new StreamContextStatusRequested
            {
                Id = id
            }).ConfigureAwait(false)).Message;
        if (streamContextStatus.Faulted)
            return StreamContextResponse.CreateFaulted(
                new StatusResponse(streamContextStatus.Completed,
                streamContextStatus.ErrorCode,
                streamContextStatus.ErrorMessage));

        var streamContext = await _aggregateReader
            .Load<StreamContext, StreamContextState>(
                GetStreamName(new StreamContextId(id)), cancellationToken)
            .ConfigureAwait(false);

        if (streamContext == null)
            return null;

        return new StreamContextResponse(streamContext.State.Id.Value,
            streamContext.State.Title, streamContext.State.Description,
            streamContext.State.Removed,
            streamContext.State.StreamEventTypes.Select(_ => _).ToList(),
            new StatusResponse(streamContextStatus.Completed,
                streamContextStatus.ErrorCode,
                streamContextStatus.ErrorMessage));
    }

    static StreamName GetStreamName(StreamContextId id) =>
        StreamName.For<StreamContext, StreamContextState, StreamContextId>(id);
}