using Domain.Catalog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Catalog.EntityConfigurations;

public class PositionEntityConfiguration : IEntityTypeConfiguration<Position>
{
    public void Configure(EntityTypeBuilder<Position> builder)
    {
        builder.ToTable("positions");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .UseHiLo("position_hilo");

        builder.Property(p => p.Name)
            .IsRequired();
    }
}