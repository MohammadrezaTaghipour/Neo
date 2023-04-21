using Neo.Infrastructure.Framework.Persistence;
using System.Diagnostics;

namespace Neo.Infrastructure.Framework.Subscriptions.Contexts;

public class MessageConsumeContext : IMessageConsumeContext
{
    public MessageConsumeContext(
        string eventId,
        string eventType,
        string contentType,
        string stream,
        ulong streamPosition,
        ulong globalPosition,
        ulong sequence,
        DateTime created,
        object? message,
        Metadata? metadata,
        string subscriptionId,
        CancellationToken cancellationToken
    )
    {
        MessageId = eventId;
        MessageType = eventType;
        ContentType = contentType;
        Stream = new StreamName(stream);
        StreamPosition = streamPosition;
        GlobalPosition = globalPosition;
        Created = created;
        Metadata = metadata;
        Sequence = sequence;
        Message = message;
        CancellationToken = cancellationToken;
        SubscriptionId = subscriptionId;
    }

    public string MessageId { get; }
    public string MessageType { get; }
    public string ContentType { get; }
    public StreamName Stream { get; }
    public ulong StreamPosition { get; }
    public ulong GlobalPosition { get; }
    public DateTime Created { get; }
    public Metadata? Metadata { get; }
    public object? Message { get; }
    public ContextItems Items { get; } = new();
    public ActivityContext? ParentContext { get; set; }
    public CancellationToken CancellationToken { get; set; }
    public ulong Sequence { get; }
    public string SubscriptionId { get; }
}