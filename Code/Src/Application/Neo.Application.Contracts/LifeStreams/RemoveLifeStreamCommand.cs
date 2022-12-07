using Neo.Infrastructure.Framework.Application;

namespace Neo.Application.Contracts.LifeStreams;

public class RemoveLifeStreamCommand : BaseCommand
{
    public Guid Id { get; set; }
    public long Version { get; set; }
}
