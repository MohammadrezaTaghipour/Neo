using Neo.Infrastructure.Framework.Domain;

namespace Neo.Infrastructure.Framework.Persistence;

public interface IEventSourcedRepository<TAggregate, TState, TId>
    where TAggregate : EventSourcedAggregate<TState>
    where TState : AggregateState<TState>, new()
    where TId : AggregateId
{
    Task Add(TAggregate aggregate, CancellationToken cancellationToken);
    Task<TAggregate> GetById(TId id, CancellationToken cancellationToken);
    Task<TAggregate> GetBy(TId id, long version, CancellationToken cancellationToken);
}