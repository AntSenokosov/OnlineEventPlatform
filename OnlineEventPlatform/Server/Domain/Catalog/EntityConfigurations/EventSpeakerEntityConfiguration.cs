using Domain.Catalog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Catalog.EntityConfigurations;

public class EventSpeakerEntityConfiguration : IEntityTypeConfiguration<EventSpeaker>
{
    public void Configure(EntityTypeBuilder<EventSpeaker> builder)
    {
        builder.HasKey(k => k.Id);

        builder.Property(p => p.Id)
            .UseHiLo("eventspeakers_hilo");

        builder.HasOne(o => o.OnlineEvent)
            .WithMany(m => m.Speakers)
            .HasForeignKey(f => f.EventId);

        builder.HasOne(o => o.Speaker)
            .WithMany(m => m.Events)
            .HasForeignKey(f => f.SpeakerId);
    }
}