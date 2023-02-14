
namespace Neo.Infrastructure.Framework.Application;

public interface IApplicationService<in T> 
{
    Task Handle(T command, CancellationToken cancellationToken);
}
