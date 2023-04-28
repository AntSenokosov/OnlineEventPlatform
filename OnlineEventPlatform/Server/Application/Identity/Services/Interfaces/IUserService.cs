using Google.Authenticator;

namespace Application.Identity.Services.Interfaces;

public interface IUserService
{
    public Task<UserDto?> GetUserAsync();
    public Task<int> CreateUser(string email, string password);
    public Task<int?> UpdateUser(string email, string password, string googleAuthCode);
    public Task<bool> DeleteUser();
    public Task<string> Login(string email, string password, string googleAuthCode);
    public Task<SetupCode?> GenerateTwoFactorAuth(bool retry, string password);
    public Task<int?> VerifyTwoFactorAuth(string googleAuthCode);
    public Task<int?> DisableTwoFactorAuth(string password, string googleAuthCode);
    public Task<bool?> CheckTwoFactorAuth();
}