namespace Neo.Infrastructure.Framework.Domain;

public abstract class AggregateId
{
    protected AggregateId(string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new Exception("Invalid argument for aggregateId.");
        Value = value;
    }

    public string Value { get; }

    public sealed override string ToString() => Value;

    public static implicit operator string(AggregateId? id)
    {
        return id?.ToString() ?? throw new Exception("");
    }
}