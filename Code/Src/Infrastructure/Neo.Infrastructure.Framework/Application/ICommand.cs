namespace Neo.Infrastructure.Framework.Application;

public interface ICommand
{
    Guid CorrelationId { get; }
}

public abstract class BaseCommand : ICommand
{
    protected BaseCommand()
    {
        CorrelationId = new Guid();
    }

    public Guid CorrelationId { get; }
}