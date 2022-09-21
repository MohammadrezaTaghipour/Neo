using Neo.Infrastructure.Framework.Domain;

namespace Neo.Infrastructure.Framework.Persistence
{
    public interface IAggregateReader
    {
        Task<T> Load<T, TState>(StreamName streamName,
            CancellationToken cancellationToken)
        where T : EventSourcedAggregate<TState>
        where TState : AggregateState<TState>, new();
    }

    public class AggregateReader : IAggregateReader
    {
        private readonly IEventReader _eventReader;
        private readonly IDomainEventFactory _domainEventFactory;

        public AggregateReader(IEventReader eventReader,
            IDomainEventFactory domainEventFactory)
        {
            _eventReader = eventReader;
            _domainEventFactory = domainEventFactory;
        }

        public async Task<T> Load<T, TState>(StreamName streamName,
            CancellationToken cancellationToken)
            where T : EventSourcedAggregate<TState>
            where TState : AggregateState<TState>, new()
        {
            var resolvedEvents = await _eventReader.ReadEvents(
                streamName, 4096, cancellationToken)
            .ConfigureAwait(false);
            var domainEvents = _domainEventFactory.Create(resolvedEvents);
            return AggregateFactory.Create<T, TState>(domainEvents);
        }
    }
}
