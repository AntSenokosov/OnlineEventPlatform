namespace Application.Catalog;

public class SpeakerDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Position { get; set; }
    public string? ShortDescription { get; set; }
    public string? LongDescription { get; set; }
}