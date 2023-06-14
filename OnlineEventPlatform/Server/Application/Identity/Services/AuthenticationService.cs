using Application.Identity.Repositories.Interfaces;
using Application.Identity.Services.Interfaces;
using AutoMapper;
using Infrastructure.Database;
using Infrastructure.Exceptions;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Identity.Services;

public class AuthenticationService : BaseDataService<OnlineEventContext>, IAuthenticationService
{
    private readonly IAuthenticationRepository _authRepository;
    private readonly IMapper _mapper;
    public AuthenticationService(
        IDbContextWrapper<OnlineEventContext> dbContextWrapper,
        ILogger<BaseDataService<OnlineEventContext>> logger,
        IAuthenticationRepository authRepository,
        IMapper mapper)
        : base(dbContextWrapper, logger)
    {
        _authRepository = authRepository;
        _mapper = mapper;
    }

    public async Task<int> RegisterAsync(string firstName, string lastName, string email, string password)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var user = await _authRepository.RegisterAsync(firstName, lastName, email, password);

            return _mapper.Map<int>(user);
        });
    }

    public async Task<bool> CheckTwoFactory(string email)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var user = await _authRepository.CheckTwoFactory(email);

            return _mapper.Map<bool>(user);
        });
    }

    public async Task<UserResponseDto> Login(string email, string password, string googleAuthCode)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var user = await _authRepository.Login(email, password, googleAuthCode);

            return _mapper.Map<UserResponseDto>(user);
        });
    }

    public async Task<bool> PasswordRecovery(string email)
    {
        return await ExecuteSafeAsync(async () => await _authRepository.PasswordRecovery(email));
    }
}