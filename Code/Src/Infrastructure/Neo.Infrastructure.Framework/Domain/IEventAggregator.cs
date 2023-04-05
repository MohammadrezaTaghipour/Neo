
namespace Neo.Infrastructure.Framework.Domain;

public interface IEventAggregator
{
    void Subscribe<T>(Func<T, Task> action) where T : class;
    Task Publish<T>(T domainEvent) where T : class;
}

public class EventAggregator : IEventAggregator
{
    private readonly List<object> _subscribers = new();

    public async Task Publish<T>(T domainEvent) where T : class
    {
        var callbacks = _subscribers.OfType<Func<T, Task>>().ToArray();
        foreach (var callback in callbacks)
        {
            await callback(domainEvent);
        }
    }

    public void Subscribe<T>(Func<T, Task> callback) where T : class
    {
        _subscribers.Add(callback);
    }
}