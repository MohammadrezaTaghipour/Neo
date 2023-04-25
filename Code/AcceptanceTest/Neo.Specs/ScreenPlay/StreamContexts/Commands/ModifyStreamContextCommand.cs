using Neo.Specs.Framework;

namespace Neo.Specs.ScreenPlay.StreamContexts.Commands;

public class ModifyStreamContextCommand : ICommand
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