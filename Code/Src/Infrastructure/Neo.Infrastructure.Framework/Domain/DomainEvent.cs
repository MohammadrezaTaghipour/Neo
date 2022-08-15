namespace Neo.Infrastructure.Framework.Domain;

using System;

public class DomainEvent : IDomainEvent
{
    public DomainEvent()
    {
        EventId = Guid.NewGuid().ToString();
        PublishedOn = DateTime.UtcNow;
    }

    public string EventId { get; }
    public DateTime PublishedOn { get; }
    public int Version { get; set; }
}

