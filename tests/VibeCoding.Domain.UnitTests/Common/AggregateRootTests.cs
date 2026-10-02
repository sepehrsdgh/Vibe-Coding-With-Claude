using FluentAssertions;
using VibeCoding.Domain.Common;
using Xunit;

namespace VibeCoding.Domain.UnitTests.Common;

public class AggregateRootTests
{
    private sealed record TestEvent : IDomainEvent
    {
        public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
    }

    private sealed class TestAggregate(Guid id) : AggregateRoot<Guid>(id)
    {
        public void DoSomething() => RaiseDomainEvent(new TestEvent());
    }

    [Fact]
    public void RaiseDomainEvent_AddsEventToDomainEvents()
    {
        var aggregate = new TestAggregate(Guid.NewGuid());

        aggregate.DoSomething();

        aggregate.DomainEvents.Should().ContainSingle().Which.Should().BeOfType<TestEvent>();
    }

    [Fact]
    public void ClearDomainEvents_RemovesAllEvents()
    {
        var aggregate = new TestAggregate(Guid.NewGuid());
        aggregate.DoSomething();

        aggregate.ClearDomainEvents();

        aggregate.DomainEvents.Should().BeEmpty();
    }
}
