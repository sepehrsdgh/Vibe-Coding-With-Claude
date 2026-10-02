using VibeCoding.Domain.Common;

namespace VibeCoding.Domain.Samples;

/// <summary>
/// Example value object: a short, non-empty piece of text. Replace this whole
/// "Samples" folder with your real domain once it's defined.
/// </summary>
public sealed class Title : ValueObject
{
    public string Value { get; }

    private Title(string value)
    {
        Value = value;
    }

    public static Title Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Title must not be empty.");

        var normalized = value.Trim();
        if (normalized.Length > 200)
            throw new DomainException("Title must be 200 characters or fewer.");

        return new Title(normalized);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
