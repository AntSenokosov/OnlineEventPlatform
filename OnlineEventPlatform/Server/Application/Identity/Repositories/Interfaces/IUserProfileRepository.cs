using Domain.Identity.Entities;

namespace Application.Identity.Repositories.Interfaces;

public interface IUserProfileRepository
{
    public Task<UserProfile?> GetUserProfileAsync();
    public Task<int?> CreateUserProfileAsync(
        string firstName,
        string lastName,
        string phone);
    public Task<int?> UpdateUserProfileAsync(
        string firstName,
        string lastName,
        string phone);
}