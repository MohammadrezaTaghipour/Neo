using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Neo.Infrastructure.Framework.Subscriptions;

public class SubscriptionHostedService : IHostedService
{
    private readonly IMessageSubscription _subscription;
    private readonly ILogger<SubscriptionHostedService> _logger;
    private readonly CancellationTokenSource _subscriptionCts = new();

    public SubscriptionHostedService(IMessageSubscription subscription,
        ILoggerFactory loggerFactory)
    {
        _subscription = subscription;
        _logger = loggerFactory.CreateLogger<SubscriptionHostedService>();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug("Starting subscription {SubscriptionId}", _subscription.SubscriptionId);

        var cts = CancellationTokenSource.CreateLinkedTokenSource(
            cancellationToken,
            _subscriptionCts.Token
        );

        await _subscription.Subscribe(
            id => { },
            (id, _, ex) => { },
            cts.Token
        ).ConfigureAwait(false);

        _logger.LogInformation("Started subscription {SubscriptionId}", _subscription.SubscriptionId);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        var cts = CancellationTokenSource.CreateLinkedTokenSource(
            cancellationToken,
            _subscriptionCts.Token
        );

        await _subscription.Unsubscribe(_ => { }, cts.Token).ConfigureAwait(false);

        _logger.LogInformation("Stopped subscription {SubscriptionId}", _subscription.SubscriptionId);
    }
}