using Neo.Infrastructure.Framework.Application;

namespace Neo.Application.Contracts.StreamContexts;

public class RemoveStreamContextRequested : BaseCommand
{
    public Guid Id { get; set; }
    public long Version { get; set; }
}