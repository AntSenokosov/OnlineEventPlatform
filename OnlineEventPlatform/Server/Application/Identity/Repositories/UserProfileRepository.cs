using System.IdentityModel.Tokens.Jwt;
using System.Net;
using Application.Identity.Repositories.Interfaces;
using Domain.Identity.Entities;
using Google.Authenticator;
using Infrastructure.Database;
using Infrastructure.Exceptions;
using Infrastructure.Helpers;
using Infrastructure.Security;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Identity.Repositories;

public class UserProfileRepository : IUserProfileRepository
{
    private readonly OnlineEventContext _db;
    private readonly ICurrentUserAccessor _userAccessor;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public UserProfileRepository(
        IDbContextWrapper<OnlineEventContext> db,
        ICurrentUserAccessor userAccessor,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _db = db.DbContext;
        _userAccessor = userAccessor;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<UserResponseDto> GetCurrentUserAsync()
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Email == _userAccessor.GetCurrentEmail() && !u.IsDeleted);

        if (user == null)
        {
            throw new RestException(
            HttpStatusCode.NotFound,
            new
                {
                    Message = "User not found"
                });
        }

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

    public async Task<int?> UpdateUserAsync(string email, string firstName, string lastName, string? googleAuthCode)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Email == _userAccessor.GetCurrentEmail() && !u.IsDeleted);

        if (user == null)
        {
            return null;
        }

        user.FirstName = firstName;
        user.LastName = lastName;
        user.Email = email;

        if (user.IsGoogleAuthEnabled && googleAuthCode != null)
        {
            TwoFactorAuthHelper.TwoFactorAuthentication(user, googleAuthCode);
        }

        var userUpdate = _db.Users.Update(user);
        await _db.SaveChangesAsync();

        return userUpdate.Entity.Id;
    }

    public async Task<bool?> DeleteUser()
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Email == _userAccessor.GetCurrentEmail() && !u.IsDeleted);

        if (user == null)
        {
            return null;
        }

        user.IsDeleted = true;
        await _db.SaveChangesAsync();

        return user.IsDeleted;
    }

    public async Task<int> ChangePassword(string password)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Email == _userAccessor.GetCurrentEmail() && !u.IsDeleted);

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

    public async Task<SetupCode?> GenerateTwoFactorAuth(bool retry, string password)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Email == _userAccessor.GetCurrentEmail() && !u.IsDeleted);

        if (user == null)
        {
            return null;
        }

        if (!user.Hash.SequenceEqual(_passwordHasher.Hash(password, user.Salt)))
        {
            throw new RepositoryException("Invalid password");
        }

        if (user.IsGoogleAuthEnabled && !retry)
        {
            throw new RepositoryException("User has 2-factory authentication enable");
        }

        string key = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
        user.GoogleAuthKey = key;

        await _db.SaveChangesAsync();

        var tfa = new TwoFactorAuthenticator();
        var setupCode = tfa.GenerateSetupCode("Khai", user.Email, key, false);

        return setupCode;
    }

    public async Task<int?> VerifyTwoFactorAuth(string googleAuthCode)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Email == _userAccessor.GetCurrentEmail() && !u.IsDeleted);

        if (user == null)
        {
            return null;
        }

        if ((user.GoogleAuthKey == null) || (user.GoogleAuthKey.Length <= 0))
        {
            throw new RepositoryException("You don't have 2-factory authentication");
        }

        var tfa = new TwoFactorAuthenticator();
        var result = tfa.ValidateTwoFactorPIN(user.GoogleAuthKey, googleAuthCode);

        if (!result)
        {
            throw new RepositoryException("Invalid authenticator code!");
        }

        user.IsGoogleAuthEnabled = true;
        await _db.SaveChangesAsync();

        return user.Id;
    }

    public async Task<int?> DisableTwoFactorAuth(string password, string googleAuthCode)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Email == _userAccessor.GetCurrentEmail() && !u.IsDeleted);

        if (user == null)
        {
            return null;
        }

        if (!user.Hash.SequenceEqual(_passwordHasher.Hash(password, user.Salt)))
        {
            throw new RepositoryException("Invalid password or the authentication code");
        }

        if (user.IsGoogleAuthEnabled)
        {
            var tfa = new TwoFactorAuthenticator();
            var result = tfa.ValidateTwoFactorPIN(user.GoogleAuthKey, googleAuthCode);

            if (!result)
            {
                throw new RepositoryException("Invalid password or the authentication code");
            }
        }

        user.GoogleAuthKey = "";
        user.IsGoogleAuthEnabled = false;

        await _db.SaveChangesAsync();

        return user.Id;
    }
}