namespace Neo.Infrastructure.Framework.Application;

public interface ICommandBus
{
    Task Dispatch<T>(T command, CancellationToken cancellationToken) where T : ICommand;
}