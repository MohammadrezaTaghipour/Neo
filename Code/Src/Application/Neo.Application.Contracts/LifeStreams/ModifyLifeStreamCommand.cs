using Neo.Infrastructure.Framework.Application;

namespace Neo.Application.Contracts.LifeStreams;

public class ModifyLifeStreamCommand : BaseCommand
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public long Version { get; set; }
}
