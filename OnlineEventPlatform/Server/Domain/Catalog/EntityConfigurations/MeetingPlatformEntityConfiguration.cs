using Domain.Catalog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Catalog.EntityConfigurations;

public class MeetingPlatformEntityConfiguration : IEntityTypeConfiguration<MeetingPlatform>
{
    public void Configure(EntityTypeBuilder<MeetingPlatform> builder)
    {
        builder.HasKey(k => k.Id);

        builder.Property(p => p.Id)
            .UseHiLo("platform_hilo");
        builder.Property(p => p.Name)
            .IsRequired();
        builder.Property(p => p.Url)
            .IsRequired(false);
    }
}