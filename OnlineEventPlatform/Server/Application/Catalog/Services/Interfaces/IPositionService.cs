namespace Application.Catalog.Services.Interfaces;

public interface IPositionService
{
    public Task<IEnumerable<PositionDto>> GetPositionsAsync();
    public Task<PositionDto?> GetPositionAsync(int id);
    public Task<int> AddPositionAsync(string name);
    public Task<int?> UpdatePositionAsync(int id, string name);
    public Task<int?> RemovePositionAsync(int id);
}