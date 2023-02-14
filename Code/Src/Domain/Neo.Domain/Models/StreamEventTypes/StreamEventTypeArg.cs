using FizzWare.NBuilder;
using Neo.Domain.Contracts.StreamEventTypes;

namespace Neo.Domain.Models.StreamEventTypes;

public class StreamEventTypeArg
{
    private StreamEventTypeArg()
    {
    }

    public StreamEventTypeId Id { get; set; }
    public string Title { get; set; }
    public IReadOnlyCollection<StreamEventTypeMetadata> Metadata { get; set; }


    public static ISingleObjectBuilder<StreamEventTypeArg> Builder
        => new Builder().CreateNew<StreamEventTypeArg>();
}