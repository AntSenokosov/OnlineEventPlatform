namespace Domain.Identity.Entities;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public byte[] Hash { get; set; } = null!;
    public byte[] Salt { get; set; } = null!;
    public string? GoogleAuthKey { get; set; }
    public bool IsGoogleAuthEnabled { get; set; }
    public bool IsDeleted { get; set; }
    public UserProfile UserProfile { get; set; } = null!;
}