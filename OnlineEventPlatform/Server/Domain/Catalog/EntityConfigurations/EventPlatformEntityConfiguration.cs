using Domain.Catalog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Catalog.EntityConfigurations;

public class EventPlatformEntityConfiguration : IEntityTypeConfiguration<EventPlatform>
{
    public void Configure(EntityTypeBuilder<EventPlatform> builder)
    {
        builder.HasKey(k => k.Id);

        builder.Property(p => p.Id)
            .UseHiLo("eventplatform_hilo");
        builder.Property(p => p.Link)
            .IsRequired(false);
        builder.Property(p => p.Link)
            .IsRequired(false);
        builder.Property(p => p.LinkId)
            .IsRequired(false);
        builder.Property(p => p.Password)
            .IsRequired();

        builder.HasOne(o => o.MeetingPlatform)
            .WithMany(m => m.EventPlatforms)
            .HasForeignKey(f => f.PlatformId);
    }
}