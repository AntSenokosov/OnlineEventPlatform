namespace Api.Requests.Identity;

public class DisableTwoFactoryRequest
{
    public string Password { get; set; } = null!;
    public string GoogleAuthCode { get; set; } = null!;
}