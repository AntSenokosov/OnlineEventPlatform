using Application.Catalog.Repositories.Interfaces;
using Domain.Catalog.Entities;
using Infrastructure.Database;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Catalog.Repositories;

public class EventTypeRepository : IEventTypeRepository
{
    private readonly OnlineEventContext _context;

    public EventTypeRepository(IDbContextWrapper<OnlineEventContext> context)
    {
        _context = context.DbContext;
    }

    public async Task<IEnumerable<EventType>> GetTypes()
    {
        return await _context.TypeOfEvents.ToListAsync();
    }
}