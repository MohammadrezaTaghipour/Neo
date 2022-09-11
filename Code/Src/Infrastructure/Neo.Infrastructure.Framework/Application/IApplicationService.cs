using System.Threading.Tasks;

namespace Neo.Infrastructure.Framework.Application;

public interface IApplicationService<in T> where T : ICommand
{
    Task Handle(T command, CancellationToken cancellationToken);
}