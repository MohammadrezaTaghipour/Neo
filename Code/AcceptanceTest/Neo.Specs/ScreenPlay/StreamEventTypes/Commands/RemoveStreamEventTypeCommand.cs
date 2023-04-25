using Neo.Specs.Framework;

namespace Neo.Specs.ScreenPlay.StreamEventTypes.Commands;

public class RemoveStreamEventTypeCommand : ICommand
{
    public Guid Id { get; set; }
    public long Version { get; set; }
}