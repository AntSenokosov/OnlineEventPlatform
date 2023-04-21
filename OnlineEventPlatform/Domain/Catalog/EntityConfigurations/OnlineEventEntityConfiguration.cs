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
    }
}