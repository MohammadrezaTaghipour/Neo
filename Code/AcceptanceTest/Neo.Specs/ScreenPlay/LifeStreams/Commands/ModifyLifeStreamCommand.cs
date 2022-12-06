using Neo.Specs.Framework;

namespace Neo.Specs.ScreenPlay.LifeStreams.Commands;

public class ModifyLifeStreamCommand : ICommand
{
    public ModifyLifeStreamCommand()
    {
        ParentStreams = new List<ParentStreamCommandItem>();
    }
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public IReadOnlyCollection<ParentStreamCommandItem> ParentStreams { get; set; }
    public long Version { get; set; }
}
