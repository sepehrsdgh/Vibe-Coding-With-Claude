using FluentAssertions;
using VibeCoding.Domain.Common;
using Xunit;

namespace VibeCoding.Domain.UnitTests.Common;

public class EntityTests
{
    private sealed class TestEntity(Guid id) : Entity<Guid>(id);

    [Fact]
    public void Equals_WhenIdsMatchAndSameType_ReturnsTrue()
    {
        var id = Guid.NewGuid();
        var first = new TestEntity(id);
        var second = new TestEntity(id);

        first.Equals(second).Should().BeTrue();
        (first == second).Should().BeTrue();
    }

    [Fact]
    public void Equals_WhenIdsDiffer_ReturnsFalse()
    {
        var first = new TestEntity(Guid.NewGuid());
        var second = new TestEntity(Guid.NewGuid());

        first.Equals(second).Should().BeFalse();
        (first != second).Should().BeTrue();
    }

    [Fact]
    public void GetHashCode_WhenIdsMatch_AreEqual()
    {
        var id = Guid.NewGuid();
        var first = new TestEntity(id);
        var second = new TestEntity(id);

        first.GetHashCode().Should().Be(second.GetHashCode());
    }
}
