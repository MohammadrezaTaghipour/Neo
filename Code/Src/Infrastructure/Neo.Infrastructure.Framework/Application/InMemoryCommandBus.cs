using Microsoft.Extensions.Logging;

namespace Neo.Infrastructure.Framework.Application;

public class InMemoryCommandBus : ICommandBus
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<InMemoryCommandBus> _logger;

    public InMemoryCommandBus(IServiceProvider serviceProvider,
        ILogger<InMemoryCommandBus> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task Dispatch<T>(T command, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Handling command of type: {command.GetType().Name} started.");

            var handler = _serviceProvider
                .GetService(typeof(IApplicationService<T>)) as IApplicationService<T>;

            if (handler == null)
                throw new Exception($"Could not resolve any handler for command type: {command.GetType()}.");

            await handler.Handle(command, cancellationToken).ConfigureAwait(false);

            _logger.LogInformation($"Handling command of type: {command.GetType().Name} finished.");
        }
        catch (Exception e)
        {
            _logger.LogError($"Handling command of type: {command.GetType().Name} failed.");
            _logger.LogError(e, e.Message);
            throw;
        }
    }
}