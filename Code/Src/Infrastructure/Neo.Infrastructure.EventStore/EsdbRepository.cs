using Neo.Infrastructure.Framework.Domain;
using Neo.Infrastructure.Framework.Persistence;

namespace Neo.Infrastructure.EventStore;

public class EsdbRepository<TAggregate, TState, TId> :
    IEventSourcedRepository<TAggregate, TState, TId>
    where TAggregate : EventSourcedAggregate<TState>
    where TState : AggregateState<TState>, new()
    where TId : AggregateId
{
    // readonly EventStoreClient _client;

    public Task Add(StreamName streamName, TAggregate aggregate,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<TAggregate> GetById(StreamName streamName, TId id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<TAggregate> GetBy(StreamName streamName, TId id,
        long version, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}