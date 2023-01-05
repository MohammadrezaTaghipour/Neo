
using Neo.Domain.Contracts.LifeStreams;
using Neo.Domain.Models.StreamContexts;
using Neo.Domain.Models.StreamEventTypes;
using Neo.Infrastructure.Framework.Domain;

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

        if (arg.StreamEventTypeMetadata.Select(_ => _.Title)
                .Intersect(arg.Metadata.Select(_ => _.Key))
                .Count() != arg.StreamEventTypeMetadata.Count)
            throw new BusinessException(LifeStreamErrorCodes.SE_BR_10009);
    }

    static void GuardAginstRemovalIfAnyStreamEventsHasBeenAppendedBefore(LifeStream lifeStream)
    {
        if (lifeStream.State.StreamEvents.Any())
            throw new BusinessException(LifeStreamErrorCodes.LS_BR_10006);
    }
}
