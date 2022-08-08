using System.Threading.Tasks;

namespace Neo.Infrastructure.Framework.Application;

public interface ICommandHandler<T> where T : ICommand
{
    Task Handle(T command);
}