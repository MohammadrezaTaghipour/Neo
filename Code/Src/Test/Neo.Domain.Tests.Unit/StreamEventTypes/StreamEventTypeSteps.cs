using Neo.Domain.Contracts.StreamEventTypes;
using Neo.Domain.Models.StreamEventTypes;
using Neo.Domain.Tests.Unit.Extensions;
using Neo.Infrastructure.Framework.Domain;

namespace Neo.Domain.Tests.Unit.StreamEventTypes;

public class StreamEventTypeSteps : BaseStep
{
    private Dictionary<string, StreamEventType> _streamEventTypes = new();

    public async Task IDefineANewStreamEventTypeWithFollowingProperties(
        StreamEventTypeArg properties)
    {
        try
        {
            _streamEventTypes[properties.Title] = await StreamEventType
                .Create(properties);
        }
        catch (BusinessException e)
        {
            Exception = e;
        }
    }

    public void ICanFindStreamEventTypeWithAboveProperties(
        StreamEventTypeArg properties)
    {
        if (IsStreamEventTypeModified(properties.Title))
        {
            var expected = new StreamEventTypeModified(properties.Id,
                properties.Title, properties.Metadata);
            _streamEventTypes[properties.Title]
                .ShouldContainsEquivalencyOfDomainEvent(expected);
        }
        else
        {
            var expected = new StreamEventTypeDefined(properties.Id,
                properties.Title, properties.Metadata);
            _streamEventTypes[properties.Title]
                .ShouldContainsEquivalencyOfDomainEvent(expected);
        }
    }

    private bool IsStreamEventTypeModified(string title)
        => _streamEventTypes[title].Changes.OfType<StreamEventTypeModified>().Any();
}