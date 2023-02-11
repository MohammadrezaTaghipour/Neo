using Neo.Domain.Contracts.ReferentialPointers;
using Neo.Infrastructure.Framework.Domain;

namespace Neo.Domain.Models.ReferentialPointers;

public record ReferentialPointerState : AggregateState<ReferentialPointerState>
{
    //readonly ConcurrentDictionary<string, ReferentialPointerCounter> _pointers = new();
    //public IReadOnlyDictionary<string, ReferentialPointerCounter> Pointers => _pointers;

    public int Counter { get; private set; }
    public bool IsRemoved { get; private set; }

    public override ReferentialPointerState When(IDomainEvent eventToHandle)
    {
        return When((dynamic)eventToHandle);
    }

    private ReferentialPointerState When(ReferentialPointerDefined eventToHandle)
    {
        return this with
        {
            Counter = 0
        };
    }

    private ReferentialPointerState When(ReferentialPointerMarkedAsUsed eventToHandle)
    {
        return this with
        {
            Counter = Counter + 1
        };
    }

    private ReferentialPointerState When(ReferentialPointerMarkedAsUnused eventToHandle)
    {
        return this with
        {
            Counter = Counter - 1
        };
    }

    private ReferentialPointerState When(ReferentialPointerRemoved eventToHandle)
    {
        return this with
        {
            IsRemoved = true
        };
    }
}
