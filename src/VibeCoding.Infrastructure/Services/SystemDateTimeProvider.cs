using VibeCoding.Application.Common.Interfaces;

namespace VibeCoding.Infrastructure.Services;

public sealed class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
