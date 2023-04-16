namespace Api.Requests.Catalog;

public class SpeakerCreateUpdateRequest
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public int DepartmentId { get; set; }
    public int PositionId { get; set; }
    public string Description { get; set; } = null!;
}