
namespace Neo.Specs.ScreenPlay.StreamContexts.Questions;

public class StreamContextResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public IReadOnlyCollection<StreamEventTypeResponseItem> StreamEventTypes { get; set; }
    public long Version { get; set; }
}

public record StreamEventTypeResponseItem(Guid Id);
