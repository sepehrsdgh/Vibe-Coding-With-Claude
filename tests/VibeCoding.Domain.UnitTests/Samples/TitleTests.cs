using FluentAssertions;
using VibeCoding.Domain.Common;
using VibeCoding.Domain.Samples;
using Xunit;

namespace VibeCoding.Domain.UnitTests.Samples;

public class TitleTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WhenValueIsMissing_Throws(string? value)
    {
        var act = () => Title.Create(value!);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Create_WhenValueTooLong_Throws()
    {
        var act = () => Title.Create(new string('a', 201));

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Create_TrimsWhitespace()
    {
        var title = Title.Create("  hello  ");

        title.Value.Should().Be("hello");
    }

    [Fact]
    public void Equals_WhenValuesMatch_ReturnsTrue()
    {
        var a = Title.Create("hello");
        var b = Title.Create("hello");

        a.Should().Be(b);
    }
}
