namespace Neo.Infrastructure.Framework.Domain;

public interface IDomainEvent
{
    string EventId { get; }
    DateTime PublishedOn { get; }
    int Version { get; set; }
}

