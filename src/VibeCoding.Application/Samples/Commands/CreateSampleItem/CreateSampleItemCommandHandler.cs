using MediatR;
using VibeCoding.Application.Common.Interfaces;
using VibeCoding.Application.Samples.Dtos;
using VibeCoding.Domain.Samples;

namespace VibeCoding.Application.Samples.Commands.CreateSampleItem;

public sealed class CreateSampleItemCommandHandler(
    ISampleItemRepository repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateSampleItemCommand, SampleItemDto>
{
    public async Task<SampleItemDto> Handle(CreateSampleItemCommand request, CancellationToken cancellationToken)
    {
        var item = SampleItem.Create(Title.Create(request.Title));

        await repository.AddAsync(item, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new SampleItemDto(item.Id, item.Title.Value, item.IsCompleted);
    }
}
