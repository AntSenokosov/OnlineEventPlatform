namespace Domain.Catalog.Entities;

public class EventType
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public virtual ICollection<OnlineEvent> Events { get; set; } = new List<OnlineEvent>();
}