namespace Api.Requests.Identity;

public class CreateUserRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}