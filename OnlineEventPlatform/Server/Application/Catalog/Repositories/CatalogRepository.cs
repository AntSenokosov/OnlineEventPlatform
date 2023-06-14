using System.Net;
using Application.Catalog.Repositories.Interfaces;
using Domain.Catalog.Entities;
using Infrastructure.Database;
using Infrastructure.Helpers;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Catalog.Repositories;

public class CatalogRepository : ICatalogRepository
{
    private readonly OnlineEventContext _context;

    public CatalogRepository(IDbContextWrapper<OnlineEventContext> context)
    {
        _context = context.DbContext;
    }

    public async Task<IEnumerable<OnlineEvent>> GetCatalog()
    {
        return await _context.OnlineEvents
            .ToListAsync();
    }

    public async Task<EventItem> GetItem(int id)
    {
        var itemEvent = await _context.OnlineEvents
            .Where(o => o.Id == id)
            .Include(o => o.EventPlatform)
            .ThenInclude(ep => ep.MeetingPlatform)
            .Select(o => new EventItem
            {
                Id = o.Id,
                Name = o.Name,
                Description = o.Description,
                Date = o.Date,
                Time = o.Time,
                AboutEvent = o.AboutEvent,
                PlaceEvent = o.EventPlatform.MeetingPlatform.Name
            })
            .FirstOrDefaultAsync();

        if (itemEvent == null)
        {
            throw new RestException(HttpStatusCode.NotFound, new
            {
                Message = "Event not found"
            });
        }

        return itemEvent;
    }
}