using Application.Identity.Repositories.Interfaces;
using Application.Identity.Services.Interfaces;
using AutoMapper;
using Infrastructure.Database;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Identity.Services;

public class UserService : BaseDataService<OnlineEventContext>, IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    public UserService(
        IDbContextWrapper<OnlineEventContext> dbContextWrapper,
        ILogger<BaseDataService<OnlineEventContext>> logger,
        IUserRepository userRepository,
        IMapper mapper)
        : base(dbContextWrapper, logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserDto>> GetUsers()
    {
        return await ExecuteSafeAsync(async () =>
        {
            var users = await _userRepository.GetUsers();

            return users.Select(u => new UserDto()
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                GoogleAuthKey = u.GoogleAuthKey,
                IsAdmin = u.IsAdmin,
                IsSuperAdmin = u.IsSuperAdmin,
                IsGoogleAuthEnabled = u.IsGoogleAuthEnabled,
                IsDeleted = u.IsDeleted
            });
        });
    }

    public async Task<int> CreateUser(string firstName, string lastName, string email, string password)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var user = await _userRepository.CreateUser(firstName, lastName, email, password);

            return _mapper.Map<int>(user);
        });
    }

    public async Task<int> UpdateUserPermissionsToAdmin(int id)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var user = await _userRepository.UpdateUserPermissionsToAdmin(id);

            return _mapper.Map<int>(user);
        });
    }

    public async Task<int> UpdateUserPermissionsToUser(int id)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var user = await _userRepository.UpdateUserPermissionsToUser(id);

            return _mapper.Map<int>(user);
        });
    }

    public async Task<bool> DeleteUser(int id)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var user = await _userRepository.DeleteUser(id);

            return _mapper.Map<bool>(user);
        });
    }
}