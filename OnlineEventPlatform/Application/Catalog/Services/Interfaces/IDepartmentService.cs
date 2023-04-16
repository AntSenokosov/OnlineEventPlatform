namespace Application.Catalog.Services.Interfaces;

public interface IDepartmentService
{
    public Task<IEnumerable<DepartmentDto>> GetDepartmentsAsync();
    public Task<DepartmentDto?> GetDepartmentAsync(int id);
    public Task<int> AddDepartmentAsync(string number, string name);
    public Task<int?> UpdateDepartmentAsync(int id, string number, string name);
    public Task<int?> RemoveDepartmentAsync(int id);
}