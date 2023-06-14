namespace Domain.Catalog.Entities;

public class MeetingPlatform
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Url { get; set; } = null!;
    public virtual ICollection<EventPlatform> EventPlatforms { get; set; } = new List<EventPlatform>();
}