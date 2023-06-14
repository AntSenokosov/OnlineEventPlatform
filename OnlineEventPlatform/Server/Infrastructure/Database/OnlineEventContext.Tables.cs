using Domain.Catalog.Entities;
using Domain.Identity.Entities;
using Domain.Templates;
using Domain.UserEvents.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public partial class OnlineEventContext
{
    public DbSet<EventType> TypeOfEvents { get; set; } = null!;
    public DbSet<Speaker> Speakers { get; set; } = null!;
    public DbSet<OnlineEvent> OnlineEvents { get; set; } = null!;
    public DbSet<EventSpeaker> EventSpeakers { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<UserEvent> UserEvents { get; set; } = null!;
    public DbSet<MeetingPlatform> MeetingPlatforms { get; set; } = null!;
    public DbSet<EventPlatform> EventPlatforms { get; set; } = null!;
    public DbSet<MailTemplate> MailTemplates { get; set; } = null!;
}