using System.Text.Json.Serialization;

namespace Application.Identity;

public class UserDto
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    [JsonIgnore]
    public byte[] Hash { get; set; } = null!;
    [JsonIgnore]
    public byte[] Salt { get; set; } = null!;
    public string? GoogleAuthKey { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsSuperAdmin { get; set; }
    public bool IsGoogleAuthEnabled { get; set; }
    public bool IsDeleted { get; set; }
}