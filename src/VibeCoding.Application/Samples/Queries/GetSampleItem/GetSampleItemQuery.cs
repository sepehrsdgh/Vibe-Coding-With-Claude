using MediatR;
using VibeCoding.Application.Samples.Dtos;

namespace VibeCoding.Application.Samples.Queries.GetSampleItem;

public sealed record GetSampleItemQuery(Guid Id) : IRequest<SampleItemDto>;
