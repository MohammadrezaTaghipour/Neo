namespace Neo.Infrastructure.Framework.Domain;

public abstract class AggregateId
{
    protected AggregateId(Guid value)
    {
        if (value == Guid.Empty)
            throw new Exception("Invalid value for aggregateId.");
        Value = value;
    }

    public Guid Value { get; }

    public sealed override string ToString() => Value.ToString();

    public static implicit operator string(AggregateId? id)
    {
        return id?.ToString() ?? throw new Exception("");
    }
}