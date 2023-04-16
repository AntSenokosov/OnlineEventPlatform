using Domain.Catalog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Catalog.EntityConfigurations;

public class SpeakerEntityConfiguration : IEntityTypeConfiguration<Speaker>
{
    public void Configure(EntityTypeBuilder<Speaker> builder)
    {
        builder.ToTable("speakers");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .UseHiLo("speaker_hilo");

        builder.Property(s => s.FirstName)
            .IsRequired();

        builder.Property(s => s.LastName)
            .IsRequired();

        builder.Property(s => s.Description)
            .IsRequired(false);

        builder.HasOne(s => s.Department)
            .WithMany()
            .HasForeignKey(s => s.DepartmentId);

        builder.HasOne(s => s.Position)
            .WithMany()
            .HasForeignKey(s => s.PositionId);
    }
}