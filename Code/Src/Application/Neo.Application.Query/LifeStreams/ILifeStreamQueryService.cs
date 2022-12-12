using Neo.Domain.Contracts.LifeStreams;
using Neo.Domain.Models.LifeStreams;
using Neo.Infrastructure.Framework.Application;
using Neo.Infrastructure.Framework.Persistence;

namespace Neo.Application.Query.LifeStreams;

public interface ILifeStreamQueryService : IQueryService
{
    Task<LifeStreamState> Get(Guid id, CancellationToken cancellationToken);
}

public class LifeStreamQueryService : ILifeStreamQueryService
{
    private readonly IAggregateReader _aggregateReader;

    public LifeStreamQueryService(IAggregateReader aggregateReader)
    {
        _aggregateReader = aggregateReader;
    }

    public async Task<LifeStreamState> Get(Guid id,
        CancellationToken cancellationToken)
    {
        var stream = await _aggregateReader
            .Load<LifeStream, LifeStreamState>(
                GetStreamName(new LifeStreamId(id)), cancellationToken)
            .ConfigureAwait(false);
        return stream.State;
    }

    static StreamName GetStreamName(LifeStreamId id) =>
        StreamName.For<LifeStream, LifeStreamState, LifeStreamId>(id);
}
