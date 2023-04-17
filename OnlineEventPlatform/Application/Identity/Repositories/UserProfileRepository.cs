using Application.Identity.Repositories.Interfaces;
using Domain.Identity.Entities;
using Infrastructure.Database;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Identity.Repositories;

public class UserProfileRepository : IUserProfileRepository
{
    private readonly OnlineEventContext _db;
    private readonly ICurrentUserAccessor _userAccessor;

    public UserProfileRepository(IDbContextWrapper<OnlineEventContext> db, ICurrentUserAccessor userAccessor)
    {
        _db = db.DbContext;
        _userAccessor = userAccessor;
    }

    public async Task<UserProfile?> GetUserProfileAsync()
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Email == _userAccessor.GetCurrentEmail() && !u.IsDeleted);

        if (user == null)
        {
            return null;
        }

        var userProfile = await _db.UserProfiles
            .FirstOrDefaultAsync(u => u.UserId == user.Id);

        if (userProfile == null)
        {
            return null;
        }

        return userProfile;
    }

    public async Task<int?> CreateUserProfileAsync(string firstName, string lastName, string phone)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Email == _userAccessor.GetCurrentEmail() && !u.IsDeleted);

        if (user == null)
        {
            return null;
        }

        var userProfile = new UserProfile()
        {
            UserId = user.Id,
            Email = user.Email,
            FirstName = firstName,
            LastName = lastName,
            Phone = phone
        };

        var newUserProfile = await _db.UserProfiles.AddAsync(userProfile);
        await _db.SaveChangesAsync();

        return newUserProfile.Entity.Id;
    }

    public async Task<int?> UpdateUserProfileAsync(string firstName, string lastName, string phone)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Email == _userAccessor.GetCurrentEmail() && !u.IsDeleted);

        if (user == null)
        {
            return null;
        }

        var userProfile = await _db.UserProfiles
            .FirstOrDefaultAsync(u => u.UserId == user.Id);

        if (userProfile == null)
        {
            return null;
        }

        userProfile.Email = user.Email;
        userProfile.FirstName = firstName;
        userProfile.LastName = lastName;
        userProfile.Phone = phone;

        var updateUserProfile = _db.UserProfiles.Update(userProfile);
        await _db.SaveChangesAsync();

        return updateUserProfile.Entity.Id;
    }
}