namespace Neo.Specs.ScreenPlay.StreamEvents.Commands;

public class RemoveStreamEventCommand : PartialModifyLifeStreamCommand
{
    public RemoveStreamEventCommand() :
        base(LifeStreamOperationType.RemoveStreamEvent)
    {

    }
}