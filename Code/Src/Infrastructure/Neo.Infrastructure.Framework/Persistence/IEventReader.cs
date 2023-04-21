namespace Neo.Infrastructure.Framework.Persistence;

public interface IEventReader
{
    Task<StreamEvent[]> ReadEvents(
        StreamName stream,
        int count,
        CancellationToken cancellationToken
    );
}
