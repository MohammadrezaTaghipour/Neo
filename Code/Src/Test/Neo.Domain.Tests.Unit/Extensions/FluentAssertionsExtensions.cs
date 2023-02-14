using FluentAssertions;
using Neo.Infrastructure.Framework.Domain;

namespace Neo.Domain.Tests.Unit.Extensions;

public static class FluentAssertionsExtensions
{
    public static void ShouldContainsEquivalencyOfDomainEvent<TState, TExpectation>(
        this EventSourcedAggregate<TState> aggregate,
        TExpectation expectation,
        string? because = null,
        params object[] becauseArgs)
        where TExpectation : DomainEvent
        where TState : AggregateState<TState>, new()
    {
        aggregate.Changes.First(z => z.GetType() == expectation.GetType()).Should().NotBeNull();
        aggregate.Changes.Where(z => z.GetType() == expectation.GetType()).Should()
            .ContainEquivalentOf(expectation, opt => opt
                    .Excluding(a => a.PublishedOn)
                    .Excluding(a => a.EventId)
                    .Excluding(a => a.Version)
                , because, becauseArgs);
    }
}