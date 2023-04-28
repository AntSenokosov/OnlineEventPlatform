namespace Application.Catalog;

public class SpeakerDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public int DepartmentId { get; set; }
    public int PositionId { get; set; }
    public string Description { get; set; } = null!;
}