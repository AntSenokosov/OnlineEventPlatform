namespace Domain.Catalog.Entities;

public class OnlineEvent
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime DateAndTime { get; set; }
    public string AboutEvent { get; set; } = null!;
}