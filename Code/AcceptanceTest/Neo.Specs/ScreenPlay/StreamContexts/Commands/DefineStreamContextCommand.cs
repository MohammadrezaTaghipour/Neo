using Neo.Specs.Framework;

namespace Neo.Specs.ScreenPlay.StreamContexts.Commands;

public class DefineStreamContextCommand : ICommand
{
    public DefineStreamContextCommand()
    {
        StreamEventTypes = new List<StreamEventTypeCommandItem>();
    }
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public IReadOnlyCollection<StreamEventTypeCommandItem> StreamEventTypes { get; set; }
}
