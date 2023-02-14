namespace Neo.Infrastructure.Framework.Domain;

public interface IDomainEvent
{
    Guid EventId { get; }
    DateTime PublishedOn { get; }
    long Version { get; set; }
}

