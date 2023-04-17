using Domain.Catalog.EntityConfigurations;
using Domain.Identity.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public partial class OnlineEventContext
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new DepartmentEntityConfiguration());
        builder.ApplyConfiguration(new PositionEntityConfiguration());
        builder.ApplyConfiguration(new SpeakerEntityConfiguration());
        builder.ApplyConfiguration(new OnlineEventEntityConfiguration());
        builder.ApplyConfiguration(new UserEntityConfiguration());
        builder.ApplyConfiguration(new UserProfileEntityConfiguration());
    }
}