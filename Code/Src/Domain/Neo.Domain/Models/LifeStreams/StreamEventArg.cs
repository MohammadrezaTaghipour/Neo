using FizzWare.NBuilder;
using Neo.Domain.Contracts.LifeStreams;
using Neo.Domain.Contracts.StreamEventTypes;
using Neo.Domain.Models.StreamContexts;
using Neo.Domain.Models.StreamEventTypes;

namespace Neo.Domain.Models.LifeStreams;

public class StreamEventArg
{
    private StreamEventArg() { }

    public StreamEventId Id { get; set; }
    public IStreamContext StreamContext { get; set; }
    public IStreamEventType StreamEventType { get; set; }
    public IReadOnlyCollection<LifeStreamEventMetada> Metadata { get; set; }
    public IReadOnlyCollection<StreamEventTypeMetadata> StreamEventTypeMetadata { get; set; }

    public static ISingleObjectBuilder<StreamEventArg> Builder
       => new Builder().CreateNew<StreamEventArg>();
}
