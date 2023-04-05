
namespace Neo.Infrastructure.Framework.Domain;

public abstract class DomainEvent : IDomainEvent
{
    protected DomainEvent()
    {
        EventId = Guid.NewGuid();
        PublishedOn = DateTime.UtcNow;
    }

    public Guid EventId { get; }
    public DateTime PublishedOn { get; }
    public long Version { get; set; }
}