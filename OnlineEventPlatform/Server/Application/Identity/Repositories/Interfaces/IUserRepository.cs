using Domain.Identity.Entities;
using Google.Authenticator;

namespace Application.Identity.Repositories.Interfaces;

public interface IUserRepository
{
    public Task<User?> GetUserAsync();
    public Task<int> CreateUser(string email, string password);
    public Task<int?> UpdateUser(string email, string password, string googleAuthCode);
    public Task<bool?> DeleteUser();
    public Task<string?> Login(string email, string password, string googleAuthCode);
    public Task<SetupCode?> GenerateTwoFactorAuth(bool retry, string password);
    public Task<int?> VerifyTwoFactorAuth(string googleAuthCode);
    public Task<int?> DisableTwoFactorAuth(string password, string googleAuthCode);
    public Task<bool?> CheckTwoFactorAuth();
}