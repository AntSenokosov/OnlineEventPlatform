using Domain.Catalog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Catalog.EntityConfigurations;

public class OnlineEventEntityConfiguration : IEntityTypeConfiguration<OnlineEvent>
{
    public void Configure(EntityTypeBuilder<OnlineEvent> builder)
    {
        builder.ToTable("events");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .UseHiLo("event_hilo");

        builder.Property(e => e.Name)
            .IsRequired();

        builder.Property(e => e.Description)
            .IsRequired();

        builder.Property(e => e.AboutEvent)
            .IsRequired();

        builder.HasMany(e => e.Speakers)
            .WithMany(s => s.OnlineEvents)
            .UsingEntity<Dictionary<string, object>>(
                "SpeakerEvent",
                j =>
                    j.HasOne<Speaker>()
                        .WithMany()
                        .HasForeignKey("SpeakerId"),
                j =>
                    j.HasOne<OnlineEvent>()
                        .WithMany()
                        .HasForeignKey("EventId"));
    }
}