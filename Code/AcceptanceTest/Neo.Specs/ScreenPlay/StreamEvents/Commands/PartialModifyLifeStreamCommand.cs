using Neo.Specs.Framework;

namespace Neo.Specs.ScreenPlay.StreamEvents.Commands;

public abstract class PartialModifyLifeStreamCommand : ICommand
{
    public LifeStreamOperationType OperationType { get; protected set; }
    public string RequestId { get; set; }
    public long Id { get; set; }
    public Guid LifeStreamId { get; set; }

    protected PartialModifyLifeStreamCommand(
        LifeStreamOperationType operationType)
    {
        OperationType = operationType;
    }
}
