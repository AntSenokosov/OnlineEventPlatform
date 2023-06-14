namespace Domain.Catalog.Entities;

public class EventPlatform
{
    public int Id { get; set; }
    public virtual OnlineEvent OnlineEvent { get; set; } = null!;
    public int EventId { get; set; }
    public virtual MeetingPlatform MeetingPlatform { get; set; } = null!;
    public int PlatformId { get; set; }
    public string? Link { get; set; }
    public string? LinkId { get; set; }
    public string? Password { get; set; }
}