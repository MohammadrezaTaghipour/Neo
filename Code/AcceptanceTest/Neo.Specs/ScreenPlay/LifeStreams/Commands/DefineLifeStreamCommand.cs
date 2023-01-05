using Neo.Specs.Framework;

namespace Neo.Specs.ScreenPlay.LifeStreams.Commands;

public class DefineLifeStreamCommand : ICommand
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}