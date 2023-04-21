namespace Domain.Catalog.Entities;

public class SpeakerEvent
{
    public int Id { get; set; }
    public OnlineEvent OnlineEvent { get; set; } = null!;
    public int OnlineEventId { get; set; }
    public Speaker Speaker { get; set; } = null!;
    public int SpeakerId { get; set; }
}