using Domain.Catalog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Catalog.EntityConfigurations;

public class EventTypeEntityConfiguration : IEntityTypeConfiguration<EventType>
{
    public void Configure(EntityTypeBuilder<EventType> builder)
    {
        builder.HasKey(k => k.Id);

        builder.Property(p => p.Id)
            .UseHiLo("type_hilo");
        builder.Property(p => p.Name)
            .IsRequired();
    }
}