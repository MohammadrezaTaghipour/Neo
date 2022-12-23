using Neo.Domain.Contracts.StreamContexts;
using Neo.Domain.Models.StreamContexts;
using Neo.Infrastructure.Framework.Application;
using Neo.Infrastructure.Framework.Persistence;

namespace Neo.Application.Query.StreamContexts;

public interface IStreamContextQueryService : IQueryService
{
    Task<StreamContextState> Get(Guid id, CancellationToken cancellationToken);
}


public class StreamContextQueryService : IStreamContextQueryService
{
    private readonly IAggregateReader _aggregateReader;

    public StreamContextQueryService(IAggregateReader aggregateReader)
    {
        _aggregateReader = aggregateReader;
    }

    public async Task<StreamContextState> Get(Guid id,
        CancellationToken cancellationToken)
    {
        var stream = await _aggregateReader
            .Load<StreamContext, StreamContextState>(
                GetStreamName(new StreamContextId(id)), cancellationToken)
            .ConfigureAwait(false);
        return stream.State;
    }

    static StreamName GetStreamName(StreamContextId id) =>
        StreamName.For<StreamContext, StreamContextState, StreamContextId>(id);
}