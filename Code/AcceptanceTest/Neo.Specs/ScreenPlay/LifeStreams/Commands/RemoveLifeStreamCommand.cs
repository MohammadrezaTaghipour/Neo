using Neo.Specs.Framework;

namespace Neo.Specs.ScreenPlay.LifeStreams.Commands;
public class RemoveLifeStreamCommand : ICommand
{
    public Guid Id { get; set; }
    public long Version { get; set; }
}
