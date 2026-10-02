using Microsoft.EntityFrameworkCore;
using VibeCoding.Domain.Samples;

namespace VibeCoding.Infrastructure.Persistence.Repositories;

public sealed class SampleItemRepository(AppDbContext dbContext) : ISampleItemRepository
{
    public async Task<SampleItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await dbContext.SampleItems.SingleOrDefaultAsync(i => i.Id == id, cancellationToken);

    public async Task AddAsync(SampleItem item, CancellationToken cancellationToken = default) =>
        await dbContext.SampleItems.AddAsync(item, cancellationToken);
}
