using System.Threading.Tasks;

namespace Neo.Infrastructure.Framework.Application;

public interface IApplicationService<T> where T : ICommand
{
    Task Handle(T command);
}