using Neo.Specs.Framework;

namespace Neo.Specs.ScreenPlay.StreamContexts.Commands;

public class RemoveStreamContextCommand : ICommand
{
    public Guid Id { get; set; }
    public long Version { get; set; }
}