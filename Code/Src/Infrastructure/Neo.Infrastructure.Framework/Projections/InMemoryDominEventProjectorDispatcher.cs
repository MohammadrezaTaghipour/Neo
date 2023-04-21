using Microsoft.Extensions.Logging;
using Neo.Infrastructure.Framework.Application;

namespace Neo.Infrastructure.Framework.Projections;

public class InMemoryDominEventProjectorDispatcher
    : IDominEventProjectorDispatcher
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<InMemoryCommandBus> _logger;

    public InMemoryDominEventProjectorDispatcher(IServiceProvider serviceProvider,
        ILogger<InMemoryCommandBus> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task Dispatch<T>(T eventToProject, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Starting handling projection of type: {eventToProject.GetType().Name}.");

            var handler = _serviceProvider
                .GetService(typeof(IDominEventProjector<T>)) as IDominEventProjector<T>;

            if (handler == null)
                throw new Exception($"Could not resolve any projector for type: {eventToProject.GetType()}.");

            await handler.Project(eventToProject, cancellationToken).ConfigureAwait(false);

            _logger.LogInformation($"Handling projection of type: {eventToProject.GetType().Name} finished successfully.");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
    }
}