using Microsoft.EntityFrameworkCore;
using VibeCoding.Domain.Samples;
using VibeCoding.Infrastructure.Persistence.Configurations;

namespace VibeCoding.Infrastructure.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<SampleItem> SampleItems => Set<SampleItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new SampleItemConfiguration());
    }
}
