using MediatR;
using VibeCoding.Application.Common.Exceptions;
using VibeCoding.Application.Samples.Dtos;
using VibeCoding.Domain.Samples;

namespace VibeCoding.Application.Samples.Queries.GetSampleItem;

public sealed class GetSampleItemQueryHandler(ISampleItemRepository repository)
    : IRequestHandler<GetSampleItemQuery, SampleItemDto>
{
    public async Task<SampleItemDto> Handle(GetSampleItemQuery request, CancellationToken cancellationToken)
    {
        var item = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(SampleItem), request.Id);

        return new SampleItemDto(item.Id, item.Title.Value, item.IsCompleted);
    }
}
