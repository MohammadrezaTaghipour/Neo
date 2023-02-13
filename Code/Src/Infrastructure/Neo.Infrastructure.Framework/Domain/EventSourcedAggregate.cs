namespace Neo.Infrastructure.Framework.Domain;

public abstract class EventSourcedAggregate<T> : Aggregate<T>
    where T : AggregateState<T>, new()
{
    /// <summary>
    /// The collection of previously persisted events
    /// </summary>
    private IDomainEvent[] _original { get; set; } = Array.Empty<IDomainEvent>();

    private readonly List<IDomainEvent> _pendingChanges = new();

    /// <summary>
    /// Get the list of pending changes (new events) within the scope of the current operation.
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> Changes => _pendingChanges.AsReadOnly();

    /// <summary>
    /// A collection with all the aggregate events, previously persisted and new
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> Current => _original.Concat(_pendingChanges).ToList();

    public long OriginalVersion => _original.Length - 1;
    protected void ClearChanges() => _pendingChanges.Clear();

    private void AddChange(IDomainEvent evt)
    {
        evt.Version = CurrentVersion + 1;
        _pendingChanges.Add(evt);
    }

    protected sealed override (T PreviousState, T CurrentState) Apply(IDomainEvent eventToHandle)
    {
        AddChange(eventToHandle);
        return base.Apply(eventToHandle);
    }

    public void Load(IReadOnlyCollection<IDomainEvent> events)
    {
        _original = events.ToArray();
        foreach (var domainEvent in events)
        {
            base.Apply(domainEvent);
        }
    }
}