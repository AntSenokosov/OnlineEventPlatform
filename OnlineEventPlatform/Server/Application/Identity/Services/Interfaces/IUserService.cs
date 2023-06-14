namespace Application.Identity.Services.Interfaces;

public interface IUserService
{
    public Task<IEnumerable<UserDto>> GetUsers();

    public Task<int> CreateUser(
        string firstName,
        string lastName,
        string email,
        string password);
    public Task<int> UpdateUserPermissionsToAdmin(int id);
    public Task<int> UpdateUserPermissionsToUser(int id);
    public Task<bool> DeleteUser(int id);
}