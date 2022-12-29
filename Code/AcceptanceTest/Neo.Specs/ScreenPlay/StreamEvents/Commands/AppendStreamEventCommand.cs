using Neo.Specs.Framework;

namespace Neo.Specs.ScreenPlay.StreamEvents.Commands;

public class AppendStreamEventCommand : ICommand
{
    public AppendStreamEventCommand()
    {
        Metadata = new List<StreamEventMetadaCommandItem>();
    }

    public LifeStreamOperationType OperationType => LifeStreamOperationType.AppendStreamEvent;
    public long Id { get; set; }
    public Guid LifeStreamId { get; set; }
    public Guid StreamContextId { get; set; }
    public Guid StreamEventTypeId { get; set; }
    public IReadOnlyCollection<StreamEventMetadaCommandItem> Metadata { get; set; } 
}
