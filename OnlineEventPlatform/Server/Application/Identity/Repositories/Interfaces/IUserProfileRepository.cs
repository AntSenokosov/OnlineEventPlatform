using Domain.Identity.Entities;
using Google.Authenticator;

namespace Application.Identity.Repositories.Interfaces;

public interface IUserProfileRepository
{
    public Task<UserResponseDto> GetCurrentUserAsync();
    public Task<int?> UpdateUserAsync(
        string email,
        string firstName,
        string lastName,
        string? googleAuthCode);
    public Task<bool?> DeleteUser();
    public Task<int> ChangePassword(string password);
    public Task<SetupCode?> GenerateTwoFactorAuth(bool retry, string password);
    public Task<int?> VerifyTwoFactorAuth(string googleAuthCode);
    public Task<int?> DisableTwoFactorAuth(string password, string googleAuthCode);
}