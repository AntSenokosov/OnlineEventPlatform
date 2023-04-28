using Domain.Identity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Identity.EntityConfigurations;

public class UserProfileEntityConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.ToTable("UserProfiles");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.FirstName)
            .IsRequired();

        builder.Property(u => u.LastName)
            .IsRequired();

        builder.Property(u => u.Email)
            .IsRequired();

        builder.Property(u => u.Phone)
            .IsRequired(false);
    }
}