using Domain.Identity.Entities;

namespace Application.Identity.Repositories.Interfaces;

public interface IUserRepository
{
    public Task<IEnumerable<User>> GetUsers();

    public Task<int> CreateUser(
        string firstName,
        string lastName,
        string email,
        string password);
    public Task<int> UpdateUserPermissionsToAdmin(int id);
    public Task<int> UpdateUserPermissionsToUser(int id);
    public Task<bool> DeleteUser(int id);
}