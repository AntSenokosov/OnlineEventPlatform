using System.Net;
using Application.Identity.Repositories.Interfaces;
using Domain.Identity.Entities;
using Infrastructure.Database;
using Infrastructure.Helpers;
using Infrastructure.Security;
using Microsoft.EntityFrameworkCore;

namespace Application.Identity.Repositories;

public class UserRepository : IUserRepository
{
    private readonly OnlineEventContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public UserRepository(OnlineEventContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<IEnumerable<User>> GetUsers()
    {
        return await _context.Users
            .Where(u => !u.IsDeleted && !u.IsSuperAdmin)
            .ToListAsync();
    }

    public async Task<int> CreateUser(string firstName, string lastName, string email, string password)
    {
        if (await _context.Users.AnyAsync(u => u.Email == email && !u.IsDeleted))
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

        var newUser = await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return newUser.Entity.Id;
    }

    public async Task<int> UpdateUserPermissionsToAdmin(int id)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
        {
            throw new RestException(HttpStatusCode.BadRequest, new
            {
                Message = "User not found"
            });
        }

        user.IsAdmin = true;
        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return user.Id;
    }

    public async Task<int> UpdateUserPermissionsToUser(int id)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
        {
            throw new RestException(HttpStatusCode.BadRequest, new
            {
                Message = "User not found"
            });
        }

        user.IsAdmin = false;
        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return user.Id;
    }

    public async Task<bool> DeleteUser(int id)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return false;
        }

        return true;
    }
}