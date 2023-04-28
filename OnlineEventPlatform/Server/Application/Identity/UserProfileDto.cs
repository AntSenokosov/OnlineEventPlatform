using System.Text.Json.Serialization;

namespace Application.Identity;

public class UserProfileDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    [JsonIgnore]
    public int UserId { get; set; }
}