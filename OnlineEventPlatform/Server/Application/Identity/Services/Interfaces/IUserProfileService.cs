namespace Application.Identity.Services.Interfaces;

public interface IUserProfileService
{
    public Task<UserProfileDto?> GetUserProfileAsync();
    public Task<int?> CreateUserProfileAsync(
        string firstName,
        string lastName,
        string phone);
    public Task<int?> UpdateUserProfileAsync(
        string firstName,
        string lastName,
        string phone);
}