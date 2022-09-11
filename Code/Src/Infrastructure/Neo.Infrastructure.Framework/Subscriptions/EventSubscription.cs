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

    protected CancellationTokenSource Stopping { get; } = new();

    private OnSubscribed? _onSubscribed;
    private OnDropped? _onDropped;


    public EventSubscription(T options, IMessageConsumer messageConsumer)
    {
        Options = options;
        MessageConsumer = messageConsumer;
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
        onSubscribed(Options.SubscriptionId);
    }

    public async Task Unsubscribe(OnUnsubscribed onUnsubscribed,
        CancellationToken cancellationToken)
    {
        IsRunning = false;
        await Unsubscribe(cancellationToken).ConfigureAwait(false);
        // _logger.Info(Options.SubscriptionId);
        onUnsubscribed(Options.SubscriptionId);
        await Finalize(cancellationToken);
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
                // _logger.Info(Options.SubscriptionId);

                await Subscribe(cancellationToken).ConfigureAwait(false);

                IsDropped = false;
                _onSubscribed?.Invoke(Options.SubscriptionId);

                // _logger.Info(Options.SubscriptionId);
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception e)
            {
                // _logger.Info(Options.SubscriptionId, e.Message);
                await Task.Delay(1000, cancellationToken).ConfigureAwait(false);
            }
        }
    }

    protected void Dropped(DropReason reason, Exception? exception)
    {
        if (!IsRunning) return;

        IsDropped = true;
        _onDropped?.Invoke(Options.SubscriptionId, reason, exception);

        Task.Run(
            async () =>
            {
                var delay = reason == DropReason.Stopped
                    ? TimeSpan.FromSeconds(10)
                    : TimeSpan.FromSeconds(2);

                // _logger.Warn($"Will resubscribe after {delay}");

                try
                {
                    await Resubscribe(delay, Stopping.Token);
                }
                catch (Exception e)
                {
                    // _logger.Warn(e.Message);
                    throw;
                }
            }
        );
    }
    
    protected virtual Task Finalize(CancellationToken cancellationToken) => default;

    protected async Task Handler(IMessageConsumeContext context)
    {
        await MessageConsumer.Consume(context).ConfigureAwait(false);
    }
}