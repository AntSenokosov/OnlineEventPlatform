namespace Domain.Catalog.Entities;

public class Speaker
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Position { get; set; }
    public string? ShortDescription { get; set; }
    public string? LongDescription { get; set; }
    public virtual ICollection<EventSpeaker> Events { get; set; } = new List<EventSpeaker>();
}