namespace Neo.Specs.ScreenPlay.StreamEvents.Commands;

public class AppendStreamEventCommand : PartialModifyLifeStreamCommand
{
    public AppendStreamEventCommand() :
        base(LifeStreamOperationType.AppendStreamEvent)
    {
        Metadata = new List<StreamEventMetadaCommandItem>();
    }

    public Guid StreamContextId { get; set; }
    public Guid StreamEventTypeId { get; set; }
    public IReadOnlyCollection<StreamEventMetadaCommandItem> Metadata { get; set; }
}
