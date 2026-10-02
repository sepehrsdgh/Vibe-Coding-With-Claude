using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VibeCoding.Application.Common.Interfaces;
using VibeCoding.Domain.Samples;
using VibeCoding.Infrastructure.Persistence;
using VibeCoding.Infrastructure.Persistence.Repositories;
using VibeCoding.Infrastructure.Services;

namespace VibeCoding.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default") ?? "Data Source=app.db";

        services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));

        services.AddScoped<ISampleItemRepository, SampleItemRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();

        return services;
    }
}
