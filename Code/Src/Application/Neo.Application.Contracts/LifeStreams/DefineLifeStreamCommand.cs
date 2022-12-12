using Neo.Infrastructure.Framework.Application;

namespace Neo.Application.Contracts.LifeStreams;

public class DefineLifeStreamCommand : BaseCommand
{
    public DefineLifeStreamCommand()
    {
        ParentStreams = new List<ParentStreamCommandItem>();
    }
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public IReadOnlyCollection<ParentStreamCommandItem> ParentStreams { get; set; }
}
