namespace Api.Requests.Catalog;

public class OnlineEventCreateUpdateRequest
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime DateAndTime { get; set; }
    public string AboutEvent { get; set; } = null!;
}