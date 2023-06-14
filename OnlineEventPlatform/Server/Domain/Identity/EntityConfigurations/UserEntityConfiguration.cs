using Domain.Identity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Identity.EntityConfigurations;

public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .UseHiLo("user_hilo");

        builder.Property(u => u.Email)
            .IsRequired();

        builder.Property(u => u.FirstName)
            .IsRequired();

        builder.Property(u => u.LastName)
            .IsRequired();

        builder.Property(u => u.GoogleAuthKey)
            .IsRequired(false);
    }
}