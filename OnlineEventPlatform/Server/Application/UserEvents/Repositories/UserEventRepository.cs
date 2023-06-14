using System.Net;
using Application.UserEvents.Repositories.Interfaces;
using Domain.Catalog.Entities;
using Domain.UserEvents.Entities;
using Infrastructure.Database;
using Infrastructure.Helpers;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.UserEvents.Repositories;

public class UserEventRepository : IUserEventRepository
{
    private readonly OnlineEventContext _onlineEventContext;
    private readonly ICurrentUserAccessor _userAccessor;

    public UserEventRepository(ICurrentUserAccessor userAccessor, OnlineEventContext onlineEventContext)
    {
        _userAccessor = userAccessor;
        _onlineEventContext = onlineEventContext;
    }

    public async Task<IEnumerable<OnlineEvent>?> GetEvents()
    {
        var user = await _onlineEventContext.Users
            .FirstOrDefaultAsync(u => u.Email == _userAccessor.GetCurrentEmail());

        if (user == null)
        {
            return null;
        }

        var userEvents = await _onlineEventContext.UserEvents
            .Where(e => e.UserId == user.Id)
            .ToListAsync();

        var events = new List<OnlineEvent>();

        foreach (var userEvent in userEvents)
        {
            var onlineEvent = await _onlineEventContext.OnlineEvents
                .FirstOrDefaultAsync(e => e.Id == userEvent.OnlineEventId);

            if (onlineEvent != null)
            {
                events.Add(onlineEvent);
            }
        }

        return events;
    }

    public async Task<int?> AddEvent(int eventId)
    {
        var user = await _onlineEventContext.Users
            .FirstOrDefaultAsync(u => u.Email == _userAccessor.GetCurrentEmail());

        if (user == null)
        {
            return null;
        }

        var onlineEvent = await _onlineEventContext.OnlineEvents
            .FirstOrDefaultAsync(e => e.Id == eventId);

        if (onlineEvent == null)
        {
            return null;
        }

        var userEvent = new UserEvent()
        {
            UserId = user.Id,
            OnlineEventId = onlineEvent.Id
        };

        var newUserEvent = await _onlineEventContext.UserEvents.AddAsync(userEvent);
        await _onlineEventContext.SaveChangesAsync();

        return newUserEvent.Entity.Id;
    }

    public async Task<int?> DeleteEvent(int eventId)
    {
        var user = await _onlineEventContext.Users
            .FirstOrDefaultAsync(u => u.Email == _userAccessor.GetCurrentEmail());

        if (user == null)
        {
            return null;
        }

        var onlineEvent = await _onlineEventContext.OnlineEvents
            .FirstOrDefaultAsync(e => e.Id == eventId);

        if (onlineEvent == null)
        {
            return null;
        }

        var userEvent = await _onlineEventContext.UserEvents
            .FirstOrDefaultAsync(e => e.UserId == user.Id && e.OnlineEventId == onlineEvent.Id);

        if (userEvent == null)
        {
            return null;
        }

        var removeEntity = _onlineEventContext.UserEvents.Remove(userEvent);
        await _onlineEventContext.SaveChangesAsync();

        return removeEntity.Entity.Id;
    }

    public async Task<bool> Check(int eventId)
    {
        var user = await _onlineEventContext.Users
            .FirstOrDefaultAsync(u => u.Email == _userAccessor.GetCurrentEmail());

        if (user == null)
        {
            throw new RestException(
                HttpStatusCode.NotFound,
                new
                {
                    Message = "User not found"
                });
        }

        var userEvent = await _onlineEventContext.UserEvents
            .FirstOrDefaultAsync(ue => ue.UserId == user.Id && ue.OnlineEventId == eventId);

        return userEvent != null;
    }
}