namespace Neo.Web.UI.Services.StreamEventTypes.Models;

public class StreamEventTypeModificationModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public long Version { get; set; }
    public IReadOnlyList<StreamEventTypeMetadataItem> Metadata { get; set; }
}
