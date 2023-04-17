using Application.Identity.Repositories.Interfaces;
using Application.Identity.Services.Interfaces;
using AutoMapper;
using Infrastructure.Database;
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

    public async Task<UserProfileDto?> GetUserProfileAsync()
    {
        return await ExecuteSafeAsync(async () =>
        {
            var user = await _profileRepository.GetUserProfileAsync();

            if (user == null)
            {
                return null;
            }

            var userDto = new UserProfileDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone
            };

            return userDto;
        });
    }

    public async Task<int?> CreateUserProfileAsync(string firstName, string lastName, string phone)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var user = await _profileRepository.CreateUserProfileAsync(firstName, lastName, phone);
            return _mapper.Map<int?>(user);
        });
    }

    public async Task<int?> UpdateUserProfileAsync(string firstName, string lastName, string phone)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var user = await _profileRepository.UpdateUserProfileAsync(firstName, lastName, phone);
            return _mapper.Map<int?>(user);
        });
    }
}