using System.Net;
using Application.Identity.Repositories.Interfaces;
using Domain.Identity.Entities;
using Google.Authenticator;
using Infrastructure.Database;
using Infrastructure.Helpers;
using Infrastructure.Security;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Identity.Repositories;

public class UserRepository : IUserRepository
{
    private readonly OnlineEventContext _db;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ICurrentUserAccessor _userAccessor;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public UserRepository(
        IDbContextWrapper<OnlineEventContext> db,
        IPasswordHasher passwordHasher,
        ICurrentUserAccessor userAccessor,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _db = db.DbContext;
        _passwordHasher = passwordHasher;
        _userAccessor = userAccessor;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<User?> GetUserAsync()
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Email == _userAccessor.GetCurrentEmail() && !u.IsDeleted);

        if (user == null)
        {
            return null;
        }

        return user;
    }

    public async Task<int> CreateUser(string email, string password)
    {
        var salt = Guid.NewGuid().ToByteArray();

        var user = new User()
        {
            Email = email,
            Hash = _passwordHasher.Hash(password, salt),
            Salt = salt
        };

        var newUser = await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();

        return newUser.Entity.Id;
    }

    public async Task<int?> UpdateUser(string email, string password, string googleAuthCode)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Email == _userAccessor.GetCurrentEmail() && !u.IsDeleted);

        if (user == null)
        {
            return null;
        }

        if (!string.IsNullOrWhiteSpace(password))
        {
            var salt = Guid.NewGuid().ToByteArray();
            user.Hash = _passwordHasher.Hash(user.Email, salt);
        }

        TwoFactorAuthHelper.TwoFactorAuthentication(user, googleAuthCode);

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

    public async Task<string?> Login(string email, string password, string googleAuthCode)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Email.Equals(email) && !u.IsDeleted);

        if (user == null)
        {
            return null;
        }

        if (!user.Hash.SequenceEqual(_passwordHasher.Hash(password, user.Salt)))
        {
            return null;
        }

        TwoFactorAuthHelper.TwoFactorAuthentication(user, googleAuthCode);

        var userToken = await _jwtTokenGenerator.CreateToken(user.Email, 120);

        return userToken;
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
            throw new RestException(
                HttpStatusCode.Unauthorized,
                new
                {
                    Error = "Invalid password"
                });
        }

        if (user.IsGoogleAuthEnabled && !retry)
        {
            throw new RestException(
                HttpStatusCode.BadRequest,
                new
                {
                    Error = "User has 2-factory authentication enable"
                });
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
            throw new RestException(
                HttpStatusCode.Unauthorized,
                new
                {
                    Error = "You don't have 2-factory authentication"
                });
        }

        var tfa = new TwoFactorAuthenticator();
        var result = tfa.ValidateTwoFactorPIN(user.GoogleAuthKey, googleAuthCode);

        if (!result)
        {
            throw new RestException(HttpStatusCode.BadRequest, new { Error = "Invalid authenticator code!" });
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
            throw new RestException(
                HttpStatusCode.Unauthorized,
                new
                {
                    Error = "Invalid password or the authentication code"
                });
        }

        if (user.IsGoogleAuthEnabled)
        {
            var tfa = new TwoFactorAuthenticator();
            var result = tfa.ValidateTwoFactorPIN(user.GoogleAuthKey, googleAuthCode);

            if (!result)
            {
                throw new RestException(
                    HttpStatusCode.Unauthorized,
                    new
                    {
                        Error = "Invalid password or the authentication code"
                    });
            }
        }

        user.GoogleAuthKey = "";
        user.IsGoogleAuthEnabled = false;

        await _db.SaveChangesAsync();

        return user.Id;
    }

    public async Task<bool?> CheckTwoFactorAuth()
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Email == _userAccessor.GetCurrentEmail() && !u.IsDeleted);

        if (user == null)
        {
            return null;
        }

        return user.IsGoogleAuthEnabled;
    }
}