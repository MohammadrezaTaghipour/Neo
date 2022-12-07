using Neo.Infrastructure.Framework.Application;

namespace Neo.Application.Contracts.LifeStreams;

public class ModifyLifeStreamCommand : BaseCommand
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
