using Neo.Infrastructure.Framework.Domain;

namespace Neo.Domain.Contracts.LifeStreams;

public class LifeStreamModified : DomainEvent
{
    public LifeStreamModified(LifeStreamId id,
        string title, string description)
    {
        Id = id;
        Title = title;
        Description = description;
    }

    public LifeStreamId Id { get; }
    public string Title { get; }
    public string Description { get; }
}