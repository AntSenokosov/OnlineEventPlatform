using Microsoft.AspNetCore.Http;

namespace Application.Catalog;

public class OnlineEventDto
{
    public int Id { get; set; }
    public int Type { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan Time { get; set; }
    public string? AboutEvent { get; set; }
    public IFormFile? Photo { get; set; }
    public IEnumerable<int>? Speakers { get; set; }
    public int Platform { get; set; }
    public string? Link { get; set; }
    public string? MeetingId { get; set; }
    public string? Password { get; set; }
}