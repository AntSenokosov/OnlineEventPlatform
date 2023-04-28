namespace Api.Requests.Identity;

public class CreateUpdateUserProfileRequest
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Phone { get; set; }
}