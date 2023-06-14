using Application.Catalog.Repositories.Interfaces;
using Domain.Catalog.Entities;
using Infrastructure.Database;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Catalog.Repositories;

public class MeetingPlatformRepository : IMeetingPlatformRepository
{
    private readonly OnlineEventContext _context;

    public MeetingPlatformRepository(IDbContextWrapper<OnlineEventContext> context)
    {
        _context = context.DbContext;
    }

    public async Task<IEnumerable<MeetingPlatform>> GetMeetingPlatforms()
    {
        return await _context.MeetingPlatforms.ToListAsync();
    }
}