using Domain.Catalog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Catalog.EntityConfigurations;

public class SpeakerEventEntityConfiguration : IEntityTypeConfiguration<SpeakerEvent>
{
    public void Configure(EntityTypeBuilder<SpeakerEvent> builder)
    {
        builder.HasKey(s => s.Id);

        builder.HasOne(s => s.Speaker)
            .WithMany()
            .HasForeignKey(f => f.SpeakerId);

        builder.HasOne(s => s.OnlineEvent)
            .WithMany()
            .HasForeignKey(f => f.OnlineEventId);
    }
}