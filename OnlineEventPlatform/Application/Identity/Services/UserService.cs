using Application.Identity.Repositories.Interfaces;
using Application.Identity.Services.Interfaces;
using AutoMapper;
using Google.Authenticator;
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

    public async Task<UserDto?> GetUserAsync()
    {
        return await ExecuteSafeAsync(async () =>
        {
            var user = await _userRepository.GetUserAsync();

            if (user == null)
            {
                return null;
            }

            var userDto = new UserDto()
            {
                Id = user.Id,
                Email = user.Email,
                GoogleAuthKey = user.GoogleAuthKey,
                IsGoogleAuthEnabled = user.IsGoogleAuthEnabled,
                IsDeleted = user.IsDeleted
            };

            return userDto;
        });
    }

    public async Task<int> CreateUser(string email, string password)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var user = await _userRepository.CreateUser(email, password);
            return _mapper.Map<int>(user);
        });
    }

    public async Task<int?> UpdateUser(string email, string password, string googleAuthCode)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var user = await _userRepository.UpdateUser(email, password, googleAuthCode);
            return _mapper.Map<int>(user);
        });
    }

    public async Task<bool> DeleteUser()
    {
        return await ExecuteSafeAsync(async () =>
        {
            var user = await _userRepository.DeleteUser();
            return _mapper.Map<bool>(user);
        });
    }

    public async Task<string> Login(string email, string password, string googleAuthCode)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var user = await _userRepository.Login(email, password, googleAuthCode);
            return _mapper.Map<string>(user);
        });
    }

    public async Task<SetupCode?> GenerateTwoFactorAuth(bool retry, string password)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var user = await _userRepository.GenerateTwoFactorAuth(retry, password);
            return _mapper.Map<SetupCode?>(user);
        });
    }

    public async Task<int?> VerifyTwoFactorAuth(string googleAuthCode)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var user = await _userRepository.VerifyTwoFactorAuth(googleAuthCode);
            return _mapper.Map<int>(user);
        });
    }

    public async Task<int?> DisableTwoFactorAuth(string password, string googleAuthCode)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var user = await _userRepository.DisableTwoFactorAuth(password, googleAuthCode);
            return _mapper.Map<int>(user);
        });
    }

    public async Task<bool?> CheckTwoFactorAuth()
    {
        return await ExecuteSafeAsync(async () =>
        {
            var user = await _userRepository.CheckTwoFactorAuth();
            return _mapper.Map<bool>(user);
        });
    }
}