

namespace Neo.Infrastructure.Framework.Application;

public interface ICommandBus
{
    Task Dispatch<T>(T command) where T: ICommand;
}