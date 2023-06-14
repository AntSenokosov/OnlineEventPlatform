namespace Domain.Identity.Entities;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public byte[] Hash { get; set; } = null!;
    public byte[] Salt { get; set; } = null!;
    public string? GoogleAuthKey { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsSuperAdmin { get; set; }
    public bool IsGoogleAuthEnabled { get; set; }
    public bool IsDeleted { get; set; }
}