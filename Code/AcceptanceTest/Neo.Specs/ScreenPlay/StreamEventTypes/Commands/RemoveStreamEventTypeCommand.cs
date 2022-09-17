using Neo.Specs.Framework;

namespace Neo.Specs.ScreenPlay.StreamEventTypes.Commands;

public class RemoveStreamEventTypeCommand : ICommand
{
    public Guid CorrelationId { get; set; }
    public Guid Id { get; set; }
    public long Version { get; set; }
}