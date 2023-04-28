namespace Api.Requests.Identity;

public class VerifyTwoFactoryRequest
{
    public string GoogleAuthCode { get; set; } = null!;
}