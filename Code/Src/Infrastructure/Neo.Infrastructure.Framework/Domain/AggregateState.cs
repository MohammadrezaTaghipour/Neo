namespace Neo.Infrastructure.Framework.Domain;

public abstract record AggregateState<T> where T : AggregateState<T>
{
    public abstract T When(IDomainEvent eventToHandle);
}