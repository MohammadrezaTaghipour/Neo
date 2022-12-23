using Neo.Infrastructure.Framework.Application;

namespace Neo.Application.Contracts.StreamContexts;

public class ModifyStreamContextCommand : BaseCommand
{
    public ModifyStreamContextCommand()
    {
        StreamEventTypes = new List<StreamEventTypeCommandItem>();
    }
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public IReadOnlyCollection<StreamEventTypeCommandItem> StreamEventTypes { get; set; }
    public long Version { get; set; }
}
