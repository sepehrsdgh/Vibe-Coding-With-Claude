using FluentAssertions;
using VibeCoding.Domain.Common;
using VibeCoding.Domain.Samples;
using Xunit;

namespace VibeCoding.Domain.UnitTests.Samples;

public class SampleItemTests
{
    [Fact]
    public void Create_SetsTitleAndIsNotCompleted()
    {
        var title = Title.Create("Buy groceries");

        var item = SampleItem.Create(title);

        item.Title.Should().Be(title);
        item.IsCompleted.Should().BeFalse();
        item.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void Rename_UpdatesTitle()
    {
        var item = SampleItem.Create(Title.Create("Old title"));
        var newTitle = Title.Create("New title");

        item.Rename(newTitle);

        item.Title.Should().Be(newTitle);
    }

    [Fact]
    public void Complete_WhenNotCompleted_MarksAsCompleted()
    {
        var item = SampleItem.Create(Title.Create("Task"));

        item.Complete();

        item.IsCompleted.Should().BeTrue();
    }

    [Fact]
    public void Complete_WhenAlreadyCompleted_Throws()
    {
        var item = SampleItem.Create(Title.Create("Task"));
        item.Complete();

        var act = item.Complete;

        act.Should().Throw<DomainException>();
    }
}
