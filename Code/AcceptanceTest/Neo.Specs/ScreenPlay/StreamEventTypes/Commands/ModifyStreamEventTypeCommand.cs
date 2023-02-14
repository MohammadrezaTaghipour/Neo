using Neo.Specs.Framework;

namespace Neo.Specs.ScreenPlay.StreamEventTypes.Commands;

public class ModifyStreamEventTypeCommand : ICommand
{
    public ModifyStreamEventTypeCommand()
    {
        Metadata = new List<StreamEventTypeMetadataCommandItem>();
    }

    public Guid Id { get; set; }
    public string Title { get; set; }
    public IReadOnlyCollection<StreamEventTypeMetadataCommandItem> Metadata { get; set; }
    public long Version { get; set; }
}