namespace Neo.Infrastructure.Framework.Subscriptions.Contexts;

public class ContextItems
{
    readonly Dictionary<string, object?> _items = new();

    public ContextItems AddItem(string key, object? value)
    {
        _items.TryAdd(key, value);
        return this;
    }

    public T? GetItem<T>(string key)
        => _items.TryGetValue(key, out var value) && value is T val
            ? val
            : default;

    public bool TryGetItem<T>(string key, out T? value)
    {
        if (_items.TryGetValue(key, out var val) && val is T val2)
        {
            value = val2;
            return true;
        }

        value = default;
        return false;
    }
}