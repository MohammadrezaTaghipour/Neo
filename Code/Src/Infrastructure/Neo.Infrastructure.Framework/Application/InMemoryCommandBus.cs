using Microsoft.Extensions.Logging;

namespace Neo.Infrastructure.Framework.Application;

public class InMemoryCommandBus : ICommandBus
{
    readonly IServiceProvider _serviceProvider;
    readonly ILogger<InMemoryCommandBus> _logger;

    public InMemoryCommandBus(IServiceProvider serviceProvider,
        ILogger<InMemoryCommandBus> logger
    )
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task Dispatch<T>(T command) where T : ICommand
    {
        _logger.LogInformation($"Starting handling command of type: {command.GetType().Name}.");

        var handler = _serviceProvider
            .GetService(typeof(ICommandHandler<T>)) as ICommandHandler<T>;

        if (handler == null)
            throw new Exception($"Could not resolve any handler for type: {command.GetType()}.");

        await handler.Handle(command).ConfigureAwait(false);

        _logger.LogInformation($"Handling command of type: {command.GetType().Name} finished successfully");
    }
}