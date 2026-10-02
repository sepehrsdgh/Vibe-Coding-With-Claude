using FluentAssertions;
using VibeCoding.Domain.Common;
using Xunit;

namespace VibeCoding.Domain.UnitTests.Common;

public class ValueObjectTests
{
    private sealed class Coordinates(int x, int y) : ValueObject
    {
        public int X { get; } = x;
        public int Y { get; } = y;

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return X;
            yield return Y;
        }
    }

    [Fact]
    public void Equals_WhenComponentsMatch_ReturnsTrue()
    {
        var a = new Coordinates(1, 2);
        var b = new Coordinates(1, 2);

        a.Equals(b).Should().BeTrue();
        (a == b).Should().BeTrue();
        a.GetHashCode().Should().Be(b.GetHashCode());
    }

    [Fact]
    public void Equals_WhenComponentsDiffer_ReturnsFalse()
    {
        var a = new Coordinates(1, 2);
        var b = new Coordinates(1, 3);

        a.Equals(b).Should().BeFalse();
        (a != b).Should().BeTrue();
    }
}
