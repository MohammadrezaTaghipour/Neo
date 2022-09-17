using Neo.Infrastructure.Framework.Application;

namespace Neo.Application.Contracts.StreamEventTypes;

public class ModifyStreamEventTypeCommand : BaseCommand
{
    public ModifyStreamEventTypeCommand()
    {
        Metadata = new List<StreamEventTypeMetadataCommandItem>();
    }

    public Guid Id { get; set; }
    public string Title { get; set; }
    public IReadOnlyList<StreamEventTypeMetadataCommandItem> Metadata { get; set; }
    public long Version { get; set; }
}