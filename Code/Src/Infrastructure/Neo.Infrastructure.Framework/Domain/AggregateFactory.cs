namespace Neo.Infrastructure.Framework.Domain;

public static class AggregateFactory
{
    public static TAggregate Create<TAggregate, TState>(
        IReadOnlyCollection<DomainEvent> domainEvents)
        where TAggregate : EventSourcedAggregate<TState>
        where TState : AggregateState<TState>, new()
    {
        var aggregate = (TAggregate)Activator.CreateInstance(typeof(TAggregate), true);
        aggregate.Load(domainEvents.Select(x => x).ToList());
        return aggregate;
    }
}
