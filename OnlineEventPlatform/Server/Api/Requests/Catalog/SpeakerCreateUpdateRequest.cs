namespace Api.Requests.Catalog;

public class SpeakerCreateUpdateRequest
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Position { get; set; }
    public string? ShortDescription { get; set; }
    public string? LongDescription { get; set; }
}