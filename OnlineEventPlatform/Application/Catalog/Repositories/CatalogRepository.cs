using Application.Catalog.Repositories.Interfaces;
using Domain.Catalog.Entities;
using Infrastructure.Database;
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
}