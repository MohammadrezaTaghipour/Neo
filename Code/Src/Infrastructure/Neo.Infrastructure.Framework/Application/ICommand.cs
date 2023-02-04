namespace Neo.Infrastructure.Framework.Application;

public interface ICommand
{
    Guid CorrelationId { get; }
}

public class CommandFalut
{
    public Guid CorrelationId { get; set; }
    public string? ErrorCode { get; set; }
    public string ErrorMessage { get; set; }
}

public abstract class BaseCommand : ICommand
{
    protected BaseCommand()
    {
        CorrelationId = new Guid();
    }

    public Guid CorrelationId { get; }
}