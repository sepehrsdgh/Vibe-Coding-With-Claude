using VibeCoding.Domain.Common;

namespace VibeCoding.Domain.Samples;

/// <summary>
/// Example aggregate root showing how Entity/AggregateRoot/ValueObject fit together.
/// Delete this once a real aggregate takes its place.
/// </summary>
public sealed class SampleItem : AggregateRoot<Guid>
{
    public Title Title { get; private set; }
    public bool IsCompleted { get; private set; }

    // Required by EF Core for materialization.
    private SampleItem() : base(Guid.Empty)
    {
        Title = null!;
    }

    private SampleItem(Guid id, Title title) : base(id)
    {
        Title = title;
        IsCompleted = false;
    }

    public static SampleItem Create(Title title) => new(Guid.NewGuid(), title);

    public void Rename(Title title) => Title = title;

    public void Complete()
    {
        if (IsCompleted)
            throw new DomainException("Sample item is already completed.");

        IsCompleted = true;
    }
}
