using Neo.Infrastructure.Framework.Application;

namespace Neo.Application.Contracts.StreamEventTypes;

public class RemoveStreamEventTypeRequested : BaseCommand
{
    public Guid Id { get; set; }
    public long Version { get; set; }
}