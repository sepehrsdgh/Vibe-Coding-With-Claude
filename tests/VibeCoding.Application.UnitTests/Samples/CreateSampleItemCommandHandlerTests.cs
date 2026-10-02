using FluentAssertions;
using Moq;
using VibeCoding.Application.Common.Interfaces;
using VibeCoding.Application.Samples.Commands.CreateSampleItem;
using VibeCoding.Domain.Samples;
using Xunit;

namespace VibeCoding.Application.UnitTests.Samples;

public class CreateSampleItemCommandHandlerTests
{
    private readonly Mock<ISampleItemRepository> _repository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly CreateSampleItemCommandHandler _handler;

    public CreateSampleItemCommandHandlerTests()
    {
        _handler = new CreateSampleItemCommandHandler(_repository.Object, _unitOfWork.Object);
    }

    [Fact]
    public async Task Handle_WithValidTitle_PersistsItemAndReturnsDto()
    {
        var command = new CreateSampleItemCommand("Buy groceries");

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Title.Should().Be("Buy groceries");
        result.IsCompleted.Should().BeFalse();
        _repository.Verify(r => r.AddAsync(It.IsAny<SampleItem>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithBlankTitle_ThrowsAndDoesNotPersist()
    {
        var command = new CreateSampleItemCommand("   ");

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<Exception>();
        _repository.Verify(r => r.AddAsync(It.IsAny<SampleItem>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
