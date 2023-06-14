namespace Api.Requests.Identity;

public class LoginRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string GoogleAuthCode { get; set; } = null!;
}