using Neo.Infrastructure.Framework.Domain;
using System.Collections.Concurrent;

namespace Neo.Infrastructure.Framework.ReferentialPointers;

public record ReferentialPointerState : AggregateState<ReferentialPointerState>
{
    readonly ConcurrentDictionary<string, ReferentialPointerCounter> _pointers = new();

    public IReadOnlyDictionary<string, ReferentialPointerCounter> Pointers => _pointers;
    public bool IsRemoved { get; private set; }

    public override ReferentialPointerState When(IDomainEvent eventToHandle)
    {
        return When((dynamic)eventToHandle);
    }

    private ReferentialPointerState When(ReferentialPointerDefined eventToHandle)
    {
        var counter = new ReferentialPointerCounter(
                eventToHandle.Id.Value, eventToHandle.PointerType);
        _pointers.TryAdd(eventToHandle.Id.Value.ToString(), counter);
        return this;
    }

    private ReferentialPointerState When(ReferentialPointerMarkedAsUsed eventToHandle)
    {
        if (_pointers.TryGetValue(eventToHandle.Id.Value.ToString(), out var value))
        {
            _pointers.TryUpdate(eventToHandle.Id.Value.ToString(),
                value.Increase(), value);
        }
        return this;
    }

    private ReferentialPointerState When(ReferentialPointerMarkedAsUnused eventToHandle)
    {
        if (_pointers.TryGetValue(eventToHandle.Id.Value.ToString(), out var value))
        {
            _pointers.TryUpdate(eventToHandle.Id.Value.ToString(),
                value.Decrease(), value);
        }
        return this;
    }

    private ReferentialPointerState When(ReferentialPointerRemoved eventToHandle)
    {
        _pointers.Clear();
        return this with
        {
            IsRemoved = true
        };
    }
}
