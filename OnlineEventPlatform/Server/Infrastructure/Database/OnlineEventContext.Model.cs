using Domain.Catalog.EntityConfigurations;
using Domain.Identity.EntityConfigurations;
using Domain.UserEvents.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public partial class OnlineEventContext
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new EventTypeEntityConfiguration());
        builder.ApplyConfiguration(new SpeakerEntityConfiguration());
        builder.ApplyConfiguration(new OnlineEventEntityConfiguration());
        builder.ApplyConfiguration(new EventPlatformEntityConfiguration());
        builder.ApplyConfiguration(new EventSpeakerEntityConfiguration());
        builder.ApplyConfiguration(new MeetingPlatformEntityConfiguration());

        builder.ApplyConfiguration(new UserEntityConfiguration());

        builder.ApplyConfiguration(new UserEventEntityConfiguration());
    }
}