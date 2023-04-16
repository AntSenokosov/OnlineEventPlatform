using Domain.Catalog.Entities;
using Application.Catalog.Repositories.Interfaces;
using Infrastructure.Database;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Catalog.Repositories;

public class PositionRepository : IPositionRepository
{
    private readonly OnlineEventContext _db;

    public PositionRepository(IDbContextWrapper<OnlineEventContext> db)
    {
        _db = db.DbContext;
    }

    public async Task<IEnumerable<Position>> GetPositionsAsync()
    {
        return await _db.Positions.ToListAsync();
    }

    public async Task<Position?> GetPositionAsync(int id)
    {
        var position = await _db.Positions
            .FirstOrDefaultAsync(p => p.Id == id);

        if (position == null)
        {
            return null;
        }

        return position;
    }

    public async Task<int> AddPositionAsync(string name)
    {
        var position = new Position()
        {
            Name = name
        };

        var newPosition = await _db.Positions.AddAsync(position);
        await _db.SaveChangesAsync();

        return newPosition.Entity.Id;
    }

    public async Task<int?> UpdatePositionAsync(int id, string name)
    {
        var position = await _db.Positions
            .FirstOrDefaultAsync(p => p.Id == id);

        if (position == null)
        {
            return null;
        }

        position.Name = name;

        var newPosition = _db.Positions.Update(position);
        await _db.SaveChangesAsync();

        return newPosition.Entity.Id;
    }

    public async Task<int?> RemovePositionAsync(int id)
    {
        var position = await _db.Positions
            .FirstOrDefaultAsync(p => p.Id == id);

        if (position == null)
        {
            return null;
        }

        var removePosition = _db.Positions.Remove(position);
        await _db.SaveChangesAsync();

        return removePosition.Entity.Id;
    }
}