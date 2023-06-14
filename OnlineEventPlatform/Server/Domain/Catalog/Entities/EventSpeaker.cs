namespace Domain.Catalog.Entities;

public class EventSpeaker
{
    public int Id { get; set; }
    public virtual OnlineEvent OnlineEvent { get; set; } = null!;
    public int EventId { get; set; }
    public virtual Speaker Speaker { get; set; } = null!;
    public int SpeakerId { get; set; }
}