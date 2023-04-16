using Domain.Catalog.Entities;
using Application.Catalog.Repositories.Interfaces;
using Infrastructure.Database;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Catalog.Repositories;

public class OnlineEventRepository : IOnlineEventRepository
{
    private readonly OnlineEventContext _db;

    public OnlineEventRepository(IDbContextWrapper<OnlineEventContext> db)
    {
        _db = db.DbContext;
    }

    public async Task<IEnumerable<OnlineEvent>> GetOnlineEventsAsync()
    {
        return await _db.OnlineEvents.ToListAsync();
    }

    public async Task<OnlineEvent?> GetOnlineEventAsync(int id)
    {
        var onlineEvent = await _db.OnlineEvents.FirstOrDefaultAsync(e => e.Id == id);

        if (onlineEvent == null)
        {
            return null;
        }

        return onlineEvent;
    }

    public async Task<int> AddOnlineEventAsync(
        string name,
        string description,
        DateTime dateTime,
        string aboutEvent)
    {
        var onlineEvent = new OnlineEvent()
        {
            Name = name,
            Description = description,
            DateAndTime = dateTime,
            AboutEvent = aboutEvent
        };

        var newEvent = await _db.OnlineEvents.AddAsync(onlineEvent);
        await _db.SaveChangesAsync();

        return newEvent.Entity.Id;
    }

    public async Task<int?> UpdateOnlineEventAsync(
        int id,
        string name,
        string description,
        DateTime dateTime,
        string aboutEvent)
    {
        var onlineEvent = await _db.OnlineEvents.FirstOrDefaultAsync(e => e.Id == id);

        if (onlineEvent == null)
        {
            return null;
        }

        onlineEvent.Name = name;
        onlineEvent.Description = description;
        onlineEvent.DateAndTime = dateTime;
        onlineEvent.AboutEvent = aboutEvent;

        var newEvent = _db.OnlineEvents.Update(onlineEvent);
        await _db.SaveChangesAsync();

        return newEvent.Entity.Id;
    }

    public async Task<int?> RemoveOnlineEventAsync(int id)
    {
        var onlineEvent = await _db.OnlineEvents.FirstOrDefaultAsync(e => e.Id == id);

        if (onlineEvent == null)
        {
            return null;
        }

        var removeEvent = _db.OnlineEvents.Remove(onlineEvent);
        await _db.SaveChangesAsync();

        return removeEvent.Entity.Id;
    }
}