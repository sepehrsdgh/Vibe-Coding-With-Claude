using Microsoft.EntityFrameworkCore;
using VibeCoding.Api.Endpoints;
using VibeCoding.Api.Middleware;
using VibeCoding.Application;
using VibeCoding.Infrastructure;
using VibeCoding.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionHandlingMiddleware();
app.UseHttpsRedirection();

app.MapSampleItemEndpoints();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
}

app.Run();
