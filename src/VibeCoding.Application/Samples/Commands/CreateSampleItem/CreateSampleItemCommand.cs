using MediatR;
using VibeCoding.Application.Samples.Dtos;

namespace VibeCoding.Application.Samples.Commands.CreateSampleItem;

public sealed record CreateSampleItemCommand(string Title) : IRequest<SampleItemDto>;
