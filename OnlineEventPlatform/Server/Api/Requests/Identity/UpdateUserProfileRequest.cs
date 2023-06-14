namespace Api.Requests.Identity;

public class UpdateUserProfileRequest
{
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? GoogleAuthCode { get; set; }
}