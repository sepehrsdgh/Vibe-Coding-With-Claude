using MediatR;
using VibeCoding.Application.Samples.Commands.CreateSampleItem;
using VibeCoding.Application.Samples.Queries.GetSampleItem;

namespace VibeCoding.Api.Endpoints;

/// <summary>
/// Example endpoints wired to the Samples use cases. Replace with your real feature endpoints.
/// </summary>
public static class SampleItemEndpoints
{
    public static void MapSampleItemEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/sample-items").WithTags("SampleItems");

        group.MapPost("/", async (CreateSampleItemCommand command, ISender sender, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(command, cancellationToken);
            return Results.Created($"/api/sample-items/{result.Id}", result);
        })
        .WithName("CreateSampleItem");

        group.MapGet("/{id:guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new GetSampleItemQuery(id), cancellationToken);
            return Results.Ok(result);
        })
        .WithName("GetSampleItem");
    }
}
