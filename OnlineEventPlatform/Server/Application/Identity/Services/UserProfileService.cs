using Application.Identity.Repositories.Interfaces;
using Application.Identity.Services.Interfaces;
using AutoMapper;
using Google.Authenticator;
using Infrastructure.Database;
using Infrastructure.Exceptions;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Identity.Services;

public class UserProfileService : BaseDataService<OnlineEventContext>, IUserProfileService
{
    private readonly IUserProfileRepository _profileRepository;
    private readonly IMapper _mapper;

    public UserProfileService(
        IDbContextWrapper<OnlineEventContext> dbContextWrapper,
        ILogger<BaseDataService<OnlineEventContext>> logger,
        IUserProfileRepository profileRepository,
        IMapper mapper)
        : base(dbContextWrapper, logger)
    {
        _profileRepository = profileRepository;
        _mapper = mapper;
    }

    public async Task<UserResponseDto> GetCurrentUserAsync()
    {
        return await ExecuteSafeAsync(async () => await _profileRepository.GetCurrentUserAsync());
    }

    public async Task<int?> UpdateUserAsync(string email, string firstName, string lastName, string? googleAuthCode)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var user = await _profileRepository.UpdateUserAsync(email, firstName, lastName, googleAuthCode);

            if (user == null)
            {
                throw new ServiceException("User not found");
            }

            return _mapper.Map<int>(user);
        });
    }

    public async Task<bool?> DeleteUser()
    {
        return await ExecuteSafeAsync(async () =>
        {
            var user = await _profileRepository.DeleteUser();

            if (user == null)
            {
                throw new ServiceException("User not found");
            }

            return _mapper.Map<bool>(user);
        });
    }

    public async Task<int> ChangePassword(string password)
    {
        return await ExecuteSafeAsync(async () => await _profileRepository.ChangePassword(password));
    }

    public async Task<SetupCode?> GenerateTwoFactorAuth(bool retry, string password)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var user = await _profileRepository.GenerateTwoFactorAuth(retry, password);

            if (user == null)
            {
                throw new ServiceException("User not found");
            }

            return _mapper.Map<SetupCode>(user);
        });
    }

    public async Task<int?> VerifyTwoFactorAuth(string googleAuthCode)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var user = await _profileRepository.VerifyTwoFactorAuth(googleAuthCode);

            if (user == null)
            {
                throw new ServiceException("User not found");
            }

            return _mapper.Map<int>(user);
        });
    }

    public async Task<int?> DisableTwoFactorAuth(string password, string googleAuthCode)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var user = await _profileRepository.DisableTwoFactorAuth(password, googleAuthCode);

            if (user == null)
            {
                throw new ServiceException("User not found");
            }

            return _mapper.Map<int>(user);
        });
    }
}