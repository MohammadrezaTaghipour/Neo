namespace Neo.Infrastructure.Framework.Domain;

public class AggregateCompletionTask
{
    private readonly List<Task> _completionTasks = new();

    public void Add(params Task[] completionTasks)
    {
        _completionTasks.AddRange(completionTasks);
    }

    public static implicit operator Task(AggregateCompletionTask completionTask)
        => Task.WhenAll(completionTask._completionTasks.ToArray());
}