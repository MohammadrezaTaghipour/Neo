using Microsoft.Extensions.Logging;
using Neo.Infrastructure.Framework.Subscriptions.Consumers;
using Neo.Infrastructure.Framework.Subscriptions.Contexts;

namespace Neo.Infrastructure.Framework.Subscriptions;

public abstract class EventSubscription<T> : IMessageSubscription
    where T : SubscriptionOptions
{
    public string SubscriptionId => Options.SubscriptionId;
    public bool IsDropped { get; set; }
    public bool IsRunning { get; set; }
    protected internal T Options { get; }
    protected IMessageConsumer MessageConsumer;
    protected readonly ILogger<EventSubscription<T>> _logger;
    protected CancellationTokenSource Stopping { get; } = new();

    private OnSubscribed? _onSubscribed;
    private OnDropped? _onDropped;



    public EventSubscription(T options, IMessageConsumer messageConsumer,
        ILoggerFactory loggerFactory)
    {
        Options = options;
        MessageConsumer = messageConsumer;
        _logger = loggerFactory.CreateLogger<EventSubscription<T>>();
    }

    public async Task Subscribe(OnSubscribed onSubscribed,
        OnDropped onDropped,
        CancellationToken cancellationToken)
    {
        var cts = CancellationTokenSource.CreateLinkedTokenSource(
            cancellationToken,
            Stopping.Token
        );
        _onSubscribed = onSubscribed;
        _onDropped = onDropped;
        await Subscribe(cts.Token).ConfigureAwait(false);
        IsRunning = true;
        _logger.LogInformation($"Subscription Started: {Options.SubscriptionId}.");
        onSubscribed(Options.SubscriptionId);
    }

    public async Task Unsubscribe(OnUnsubscribed onUnsubscribed,
        CancellationToken cancellationToken)
    {
        IsRunning = false;
        await Unsubscribe(cancellationToken).ConfigureAwait(false);
        _logger.LogWarning($"Subscription Stopped: {Options.SubscriptionId}");
        onUnsubscribed(Options.SubscriptionId);
    }

    protected abstract Task Subscribe(CancellationToken cancellationToken);

    protected abstract Task Unsubscribe(CancellationToken cancellationToken);

    protected virtual async Task Resubscribe(TimeSpan delay,
        CancellationToken cancellationToken)
    {
        await Task.Delay(delay, cancellationToken).ConfigureAwait(false);

        while (IsRunning && IsDropped && !cancellationToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogWarning($"Subscription Resubscribing: {Options.SubscriptionId}");

                await Subscribe(cancellationToken).ConfigureAwait(false);

                IsDropped = false;
                _onSubscribed?.Invoke(Options.SubscriptionId);

                _logger.LogWarning($"Subscription Restored: {Options.SubscriptionId}");

            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, $"Resubscribe Failed: {Options.SubscriptionId}");
                await Task.Delay(1000, cancellationToken).ConfigureAwait(false);
            }
        }
    }

    protected void Dropped(DropReason reason, Exception? exception)
    {
        if (!IsRunning) return;
        _logger.LogWarning(exception, $"SubscriptionDropped: {Options.SubscriptionId}", reason);

        IsDropped = true;
        _onDropped?.Invoke(Options.SubscriptionId, reason, exception);

        Task.Run(
            async () =>
            {
                var delay = reason == DropReason.Stopped
                    ? TimeSpan.FromSeconds(10)
                    : TimeSpan.FromSeconds(2);

                _logger.LogWarning($"Will resubscribe after {delay}");

                try
                {
                    await Resubscribe(delay, Stopping.Token);
                }
                catch (Exception e)
                {
                    _logger.LogWarning(e.Message);
                    throw;
                }
            }
        );
    }


    protected async Task Handler(IMessageConsumeContext context)
    {
        _logger.LogDebug($"Message Received: {context.MessageType}");
        await MessageConsumer.Consume(context).ConfigureAwait(false);
    }
}