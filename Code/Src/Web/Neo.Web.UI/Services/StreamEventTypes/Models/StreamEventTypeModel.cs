namespace Neo.Web.UI.Services.StreamEventTypes.Models;

public class StreamEventTypeModel
{
    public Guid? Id { get; set; }
    public string? Title { get; set; }
    public bool? Removed { get; set; }
    public IReadOnlyCollection<StreamEventTypeMetadataResponse>? Metadata { get; set; }
    public StatusViewModel Status { get; set; }
}


public record StreamEventTypeMetadataResponse(string Title);