namespace Domain.Catalog.Entities;

public class OnlineEvent
{
    public int Id { get; set; }
    public virtual EventType Type { get; set; } = null!;
    public int TypeId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; } = null!;
    public DateTime Date { get; set; }
    public TimeSpan Time { get; set; }
    public string? AboutEvent { get; set; } = null!;
    public byte[]? Photo { get; set; }
    public virtual ICollection<EventSpeaker> Speakers { get; set; } = new List<EventSpeaker>();
    public virtual EventPlatform EventPlatform { get; set; } = null!;
}