using System.IdentityModel.Tokens.Jwt;
using System.Net;
using Application.Identity.Repositories.Interfaces;
using Application.SendMail.Services.Interfaces;
using Domain.Identity.Entities;
using Infrastructure.Database;
using Infrastructure.Helpers;
using Infrastructure.Security;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Identity.Repositories;

public class AuthenticationRepository : IAuthenticationRepository
{
    private readonly OnlineEventContext _db;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ICurrentUserAccessor _userAccessor;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IGeneratePassword _generatePassword;
    private readonly ISendMailService _mailService;
    private readonly IUserProfileRepository _profileRepository;

    public AuthenticationRepository(
        OnlineEventContext db,
        IPasswordHasher passwordHasher,
        ICurrentUserAccessor userAccessor,
        IJwtTokenGenerator jwtTokenGenerator,
        IGeneratePassword generatePassword,
        ISendMailService mailService,
        IUserProfileRepository profileRepository)
    {
        _db = db;
        _passwordHasher = passwordHasher;
        _userAccessor = userAccessor;
        _jwtTokenGenerator = jwtTokenGenerator;
        _generatePassword = generatePassword;
        _mailService = mailService;
        _profileRepository = profileRepository;
    }

    public async Task<int> RegisterAsync(string firstName, string lastName, string email, string password)
    {
        if (await _db.Users.AnyAsync(u => u.Email == email && !u.IsDeleted))
        {
            throw new RestException(HttpStatusCode.BadRequest, new
            {
                Message = "User with this email already exists!"
            });
        }

        var salt = Guid.NewGuid().ToByteArray();

        var user = new User()
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Hash = _passwordHasher.Hash(password, salt),
            Salt = salt
        };

        var newUser = await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();

        return newUser.Entity.Id;
    }

    public async Task<bool> CheckTwoFactory(string email)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Email == _userAccessor.GetCurrentEmail() && !u.IsDeleted);

        if (user == null)
        {
            throw new RestException(HttpStatusCode.BadRequest, new
            {
                Message = "User not found"
            });
        }

        return user.IsGoogleAuthEnabled;
    }

    public async Task<UserResponseDto> Login(string email, string password, string googleAuthCode)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Email.Equals(email) && !u.IsDeleted);

        if (user == null)
        {
            throw new RestException(HttpStatusCode.Unauthorized, new { Error = "Invalid email / password." });
        }

        if (!user.Hash.SequenceEqual(_passwordHasher.Hash(password, user.Salt)))
        {
            throw new RestException(HttpStatusCode.Unauthorized, new { Error = "Invalid email / password." });
        }

        TwoFactorAuthHelper.TwoFactorAuthentication(user, googleAuthCode);

        var person = new UserResponseDto()
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            IsAdmin = user.IsAdmin,
            IsSuperAdmin = user.IsSuperAdmin,
            HasTwoFactoryAuthEnable = user.IsGoogleAuthEnabled
        };

        person.Token = await _jwtTokenGenerator.CreateToken(user.Email, 120);
        var token = new JwtSecurityTokenHandler().ReadJwtToken(person.Token);
        person.TokenValidTo = token.ValidTo;

        return person;
    }

    public async Task<bool> PasswordRecovery(string email)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Email.Equals(email) && !u.IsDeleted);

        if (user == null)
        {
            throw new RestException(HttpStatusCode.Unauthorized, new { Error = "Invalid email" });
        }

        var password = _generatePassword.GenerateRandomPassword();

        await ChangePassword(email, password);

        var result = await _mailService.SendRecoveryPassword(user, password);

        return result;
    }

    private async Task<int> ChangePassword(string email, string password)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);

        if (user == null)
        {
            throw new RestException(
                HttpStatusCode.Unauthorized,
                new
                {
                    Message = "User not found"
                });
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new RestException(
                HttpStatusCode.BadRequest,
                new
                {
                    Message = "Password will not empty"
                });
        }

        var salt = Guid.NewGuid().ToByteArray();
        user.Hash = _passwordHasher.Hash(password, salt);
        user.Salt = salt;

        _db.Users.Update(user);
        await _db.SaveChangesAsync();

        return user.Id;
    }
}