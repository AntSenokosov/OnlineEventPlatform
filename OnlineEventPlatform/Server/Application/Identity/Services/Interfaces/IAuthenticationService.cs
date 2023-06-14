namespace Application.Identity.Services.Interfaces;

public interface IAuthenticationService
{
    public Task<int> RegisterAsync(
        string firstName,
        string lastName,
        string email,
        string password);
    public Task<bool> CheckTwoFactory(string email);
    public Task<UserResponseDto> Login(string email, string password, string googleAuthCode);
    public Task<bool> PasswordRecovery(string email);
}