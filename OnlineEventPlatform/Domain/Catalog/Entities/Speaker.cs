namespace Domain.Catalog.Entities;

public class Speaker
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public Department Department { get; set; } = null!;
    public int DepartmentId { get; set; }
    public Position Position { get; set; } = null!;
    public int PositionId { get; set; }
    public string Description { get; set; } = null!;
    public List<OnlineEvent> OnlineEvents { get; set; } = new List<OnlineEvent>();
}