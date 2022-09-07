using Neo.Specs.Framework;

namespace Neo.Specs.ScreenPlay.StreamEventTypes.Commands;

public class DefineStreamEventTypeCommand : ICommand
{
    public DefineStreamEventTypeCommand()
    {
        Metadata = new List<StreamEventTypeMetadataCommandItem>();
    }
    
    public Guid Id { get; set; }
    public string Title { get; set; }
    public IReadOnlyList<StreamEventTypeMetadataCommandItem> Metadata { get; set; }
}