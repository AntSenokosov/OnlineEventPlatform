using Application.Catalog.Services.Interfaces;

namespace Api.Requests.Catalog;

public class OnlineEventCreateUpdateRequest
{
    public int Type { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime Date { get; set; }
    public TimeSpan Time { get; set; }
    public string AboutEvent { get; set; } = null!;
    public IEnumerable<int>? Speakers { get; set; }
    public int Platform { get; set; }
    public string? Link { get; set; }
    public string? MeetingId { get; set; }
    public string? Password { get; set; }
}