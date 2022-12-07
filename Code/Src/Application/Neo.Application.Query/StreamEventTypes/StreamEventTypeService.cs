using Neo.Domain.Contracts.StreamEventTypes;
using Neo.Domain.Models.StreamEventTypes;
using Neo.Infrastructure.Framework.Application;
using Neo.Infrastructure.Framework.Persistence;

namespace Neo.Application.Query.StreamEventTypes
{
    public interface IStreamEventTypeQueryService : IQueryService
    {
        Task<StreamEventTypeState> Get(Guid id, CancellationToken cancellationToken);
    }

    public class StreamEventTypeQueryService : IStreamEventTypeQueryService
    {
        private readonly IAggregateReader _aggregateReader;

        public StreamEventTypeQueryService(IAggregateReader aggregateReader)
        {
            _aggregateReader = aggregateReader;
        }

        public async Task<StreamEventTypeState> Get(Guid id,
            CancellationToken cancellationToken)
        {
            var streamEventType = await _aggregateReader
                .Load<StreamEventType, StreamEventTypeState>(
                    GetStreamName(new StreamEventTypeId(id)), cancellationToken)
                .ConfigureAwait(false);
            return streamEventType.State;
        }

        static StreamName GetStreamName(StreamEventTypeId id) =>
            StreamName.For<StreamEventType, StreamEventTypeState, StreamEventTypeId>(id);
    }
}
