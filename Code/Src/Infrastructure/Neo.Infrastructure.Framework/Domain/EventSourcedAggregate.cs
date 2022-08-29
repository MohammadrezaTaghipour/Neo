using System;
using Neo.Infrastructure.Framework.Domain;

namespace Neo.Infrastructure.Framework.Domain;

public abstract class EventSourcedAggregate<T> : Aggregate<T>
    where T : AggregateState<T>, new()
{
    readonly List<IDomainEvent> _pendingChanges = new();
    public IReadOnlyCollection<IDomainEvent> Changes => _pendingChanges.AsReadOnly();


    protected void ClearChanges() => _pendingChanges.Clear();

    protected void AddChange(IDomainEvent evt) => _pendingChanges.Add(evt);

    protected sealed override (T PreviousState, T CurrentState) Apply(IDomainEvent eventToHandle)
    {
        AddChange(eventToHandle);
        return base.Apply(eventToHandle);
    }
}