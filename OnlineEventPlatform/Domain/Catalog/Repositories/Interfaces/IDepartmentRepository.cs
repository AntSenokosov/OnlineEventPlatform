using Domain.Catalog.Entities;

namespace Domain.Catalog.Repositories.Interfaces;

public interface IDepartmentRepository
{
    public Task<IEnumerable<Department>> GetDepartmentsAsync();
    public Task<Department?> GetDepartmentAsync(int id);
    public Task<int> AddDepartmentAsync(string number, string name);
    public Task<int?> UpdateDepartmentAsync(int id, string number, string name);
    public Task<int?> RemoveDepartmentAsync(int id);
}