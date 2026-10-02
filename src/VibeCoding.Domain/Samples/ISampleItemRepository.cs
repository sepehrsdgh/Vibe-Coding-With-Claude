namespace VibeCoding.Domain.Samples;

public interface ISampleItemRepository
{
    Task<SampleItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task AddAsync(SampleItem item, CancellationToken cancellationToken = default);
}
