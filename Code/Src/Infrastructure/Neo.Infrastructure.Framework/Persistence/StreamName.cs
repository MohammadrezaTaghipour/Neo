using Neo.Infrastructure.Framework.Domain;

namespace Neo.Infrastructure.Framework.Persistence;

public class StreamName
{
    string Value { get; }

    public StreamName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(value);

        Value = value;
    }

    public static StreamName For<T, TState, TId>(TId entityId)
        where T : Aggregate<TState>
        where TState : AggregateState<TState>, new()
        where TId : AggregateId
        => new($"{typeof(T).Name}-{entityId}");

    public override string ToString() => Value;

    public static implicit operator string(StreamName streamName) => streamName.Value;
}