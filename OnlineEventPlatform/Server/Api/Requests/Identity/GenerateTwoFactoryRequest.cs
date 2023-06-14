namespace Api.Requests.Identity;

public class GenerateTwoFactoryRequest
{
    public bool Retry { get; set; }
    public string Password { get; set; } = null!;
}