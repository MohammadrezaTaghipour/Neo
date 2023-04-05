namespace Neo.Web.UI.Services.StreamEventTypes.Models;

public class StreamEventTypeDefinitionModel
{
    public StreamEventTypeDefinitionModel()
    {
        Metadata = new List<StreamEventTypeMetadataItem>();
    }

    public Guid Id { get; set; }
    public string Title { get; set; }
    public IList<StreamEventTypeMetadataItem> Metadata { get; set; } 
}

