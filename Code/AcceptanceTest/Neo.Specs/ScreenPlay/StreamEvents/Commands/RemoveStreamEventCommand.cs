using Neo.Specs.Framework;

namespace Neo.Specs.ScreenPlay.StreamEvents.Commands;

public class RemoveStreamEventCommand : ICommand
{
    public LifeStreamOperationType OperationType => LifeStreamOperationType.RemoveStreamEvent;
    public long Id { get; set; }
    public Guid LifeStreamId { get; set; }
}