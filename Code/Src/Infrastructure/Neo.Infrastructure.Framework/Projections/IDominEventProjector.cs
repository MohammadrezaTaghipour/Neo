
namespace Neo.Infrastructure.Framework.Projections;

public interface IDominEventProjector<in TEvent>
{
    Task Project(TEvent eventToProject, CancellationToken cancellationToken);
}
