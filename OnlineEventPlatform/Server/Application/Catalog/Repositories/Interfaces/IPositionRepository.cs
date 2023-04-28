using Domain.Catalog.Entities;

namespace Application.Catalog.Repositories.Interfaces;

public interface IPositionRepository
{
    public Task<IEnumerable<Position>> GetPositionsAsync();
    public Task<Position?> GetPositionAsync(int id);
    public Task<int> AddPositionAsync(string name);
    public Task<int?> UpdatePositionAsync(int id, string name);
    public Task<int?> RemovePositionAsync(int id);
}