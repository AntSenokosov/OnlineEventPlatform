using Domain.Catalog.Entities;

namespace Application.Catalog;

public class EventItem
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; } = null!;
    public DateTime Date { get; set; }
    public TimeSpan Time { get; set; }
    public string? AboutEvent { get; set; } = null!;
    public string PlaceEvent { get; set; } = null!;
    public byte[]? Photo { get; set; }
    public IEnumerable<Speaker> Speakers { get; set; } = null!;
}