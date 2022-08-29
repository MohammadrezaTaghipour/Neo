using Neo.Domain.Models.StreamEventTypes;
using Neo.Application.Contracts.StreamEventTypes;

namespace Neo.Application.StreamEventTypes;

public interface IStreamEventTypeArgFactory
{
    StreamEventTypeArg CreateFrom(DefineStreamEventTypeCommand command);
    StreamEventTypeArg CreateFrom(ModifyStreamEventTypeCommand command);
}