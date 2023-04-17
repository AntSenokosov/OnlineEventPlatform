using Domain.UserEvents.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.UserEvents.EntityConfigurations;

public class UserEventEntityConfiguration : IEntityTypeConfiguration<UserEvent>
{
    public void Configure(EntityTypeBuilder<UserEvent> builder)
    {
        builder.ToTable("userEvents");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .UseHiLo("userevent_hilo");

        builder.HasOne(o => o.User)
            .WithMany()
            .HasForeignKey(f => f.UserId);

        builder.HasOne(o => o.OnlineEvent)
            .WithMany()
            .HasForeignKey(f => f.OnlineEventId);
    }
}