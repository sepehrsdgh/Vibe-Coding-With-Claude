using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VibeCoding.Domain.Samples;

namespace VibeCoding.Infrastructure.Persistence.Configurations;

public sealed class SampleItemConfiguration : IEntityTypeConfiguration<SampleItem>
{
    public void Configure(EntityTypeBuilder<SampleItem> builder)
    {
        builder.ToTable("SampleItems");
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Title)
            .HasConversion(title => title.Value, value => Title.Create(value))
            .HasColumnName("Title")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(i => i.IsCompleted).IsRequired();

        builder.Ignore(i => i.DomainEvents);
    }
}
