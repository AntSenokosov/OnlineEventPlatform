using System.Text.Json.Serialization;

namespace Application.Identity;

public class UserDto
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    [JsonIgnore]
    public byte[] Hash { get; set; } = null!;
    [JsonIgnore]
    public byte[] Salt { get; set; } = null!;
    public string? GoogleAuthKey { get; set; }
    public bool IsGoogleAuthEnabled { get; set; }
    public bool IsDeleted { get; set; }
}