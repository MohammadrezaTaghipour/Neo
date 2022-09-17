namespace Neo.Infrastructure.Framework.Domain;

public abstract class EventSourcedAggregate<T> : Aggregate<T>
    where T : AggregateState<T>, new()
{
    private IDomainEvent[] Original { get; set; } = Array.Empty<IDomainEvent>();
    private readonly List<IDomainEvent> _pendingChanges = new();

    public IReadOnlyCollection<IDomainEvent> Current => Original.Concat(_pendingChanges).ToList();
    public IReadOnlyCollection<IDomainEvent> Changes => _pendingChanges.AsReadOnly();

    public long OriginalVersion => Original.Length - 1;
    protected void ClearChanges() => _pendingChanges.Clear();

    private void AddChange(IDomainEvent evt)
    {
        evt.Version = Version + 1;
        _pendingChanges.Add(evt);
    }

    protected sealed override (T PreviousState, T CurrentState) Apply(IDomainEvent eventToHandle)
    {
        AddChange(eventToHandle);
        return base.Apply(eventToHandle);
    }

    public void Load(IReadOnlyCollection<IDomainEvent> events)
    {
        Original = events.ToArray();
        foreach (var domainEvent in events)
        {
            base.Apply(domainEvent);
        }
    }
}