
using Neo.Domain.Contracts.LifeStreams;
using Neo.Domain.Models.StreamContexts;
using Neo.Domain.Models.StreamEventTypes;
using Neo.Infrastructure.Framework.Domain;
using Neo.Infrastructure.Framework.Subscriptions.Contexts;

namespace Neo.Domain.Models.LifeStreams;

public partial class LifeStream
{
    static void GuardAgainstRemovedLifeStream(LifeStream lifeStream)
    {
        if (lifeStream.State.Removed)
            throw new BusinessException(LifeStreamErrorCodes.SE_BR_10006);
    }

    static void GuardAgainstRemovedStreamContext(
        IStreamContext streamContext)
    {
        if (streamContext.IsRemoved())
            throw new BusinessException(LifeStreamErrorCodes.SE_BR_10007);
    }

    static void GuardAgainstRemovedStreamEventType(
        IStreamEventType streamEventTypes)
    {
        if (streamEventTypes.IsRemoved())
            throw new BusinessException(LifeStreamErrorCodes.SE_BR_10008);
    }

    static void GuardAgainstStreamEventMetadataToBeBasedOnStreamEventTypeConfiguration(
        StreamEventArg arg)
    {
        if (arg.Metadata.Count != arg.StreamEventTypeMetadata.Count)
            throw new BusinessException(LifeStreamErrorCodes.SE_BR_10009);

        if (arg.StreamEventTypeMetadata.Any(_ => arg.Metadata.Any(m => m.Key != _.Title)))
            throw new BusinessException(LifeStreamErrorCodes.SE_BR_10009);
    }
}
