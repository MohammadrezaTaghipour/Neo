namespace Neo.Infrastructure.Framework.Domain;


public abstract class Aggregate<T> where T : AggregateState<T>, new()
{
    protected Aggregate()
    {
        State = new T();
    }


    public T State { get; private set; }
    public long Version { get; set; }
    protected readonly AggregateCompletionTask CompletionTask = new();

    protected virtual (T PreviousState, T CurrentState) Apply(IDomainEvent eventToHandle)
    {
        var previous = State;
        State = State.When(eventToHandle);
        Version = Version + 1;
        return (previous, State);
    }
}

