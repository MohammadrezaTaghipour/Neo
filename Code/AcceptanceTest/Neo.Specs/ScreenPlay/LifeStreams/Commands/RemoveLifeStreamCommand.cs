using Neo.Specs.Framework;

namespace Neo.Specs.ScreenPlay.LifeStreams.Commands;
public class RemoveLifeStreamCommand : ICommand
{
    public string RequestId { get; set; }
    public Guid Id { get; set; }
    public long Version { get; set; }
}
