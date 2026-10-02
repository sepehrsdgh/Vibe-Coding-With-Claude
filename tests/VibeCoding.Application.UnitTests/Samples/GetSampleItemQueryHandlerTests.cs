using FluentAssertions;
using Moq;
using VibeCoding.Application.Common.Exceptions;
using VibeCoding.Application.Samples.Queries.GetSampleItem;
using VibeCoding.Domain.Samples;
using Xunit;

namespace VibeCoding.Application.UnitTests.Samples;

public class GetSampleItemQueryHandlerTests
{
    private readonly Mock<ISampleItemRepository> _repository = new();
    private readonly GetSampleItemQueryHandler _handler;

    public GetSampleItemQueryHandlerTests()
    {
        _handler = new GetSampleItemQueryHandler(_repository.Object);
    }

    [Fact]
    public async Task Handle_WhenItemExists_ReturnsDto()
    {
        var item = SampleItem.Create(Title.Create("Buy groceries"));
        _repository
            .Setup(r => r.GetByIdAsync(item.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(item);

        var result = await _handler.Handle(new GetSampleItemQuery(item.Id), CancellationToken.None);

        result.Id.Should().Be(item.Id);
        result.Title.Should().Be("Buy groceries");
    }

    [Fact]
    public async Task Handle_WhenItemDoesNotExist_ThrowsNotFoundException()
    {
        _repository
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((SampleItem?)null);

        var act = () => _handler.Handle(new GetSampleItemQuery(Guid.NewGuid()), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}
