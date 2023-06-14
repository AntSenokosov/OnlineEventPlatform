using Domain.Catalog.Entities;
using Domain.Identity.Entities;

namespace Domain.UserEvents.Entities;

public class UserEvent
{
    public int Id { get; set; }
    public User User { get; set; } = null!;
    public int UserId { get; set; }
    public OnlineEvent OnlineEvent { get; set; } = null!;
    public int OnlineEventId { get; set; }
}