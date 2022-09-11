using System.Diagnostics;
using Neo.Infrastructure.Framework.Persistence;

namespace Neo.Infrastructure.Framework.Subscriptions.Contexts;

public interface IMessageConsumeContext
{
    string MessageId { get; }
    string MessageType { get; }
    string ContentType { get; }
    StreamName Stream { get; }
    ulong StreamPosition { get; }
    ulong GlobalPosition { get; }
    DateTime Created { get; }
    Metadata? Metadata { get; }
    ContextItems Items { get; }
    ActivityContext? ParentContext { get; set; }
    CancellationToken CancellationToken { get; set; }
    ulong Sequence { get; }
    string SubscriptionId { get; }
    object? Message { get; }
}