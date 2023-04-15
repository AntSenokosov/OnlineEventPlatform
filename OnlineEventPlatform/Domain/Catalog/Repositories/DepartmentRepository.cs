using Domain.Catalog.Entities;
using Domain.Catalog.Repositories.Interfaces;
using Infrastructure.Database;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domain.Catalog.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly OnlineEventContext _db;

    public DepartmentRepository(IDbContextWrapper<OnlineEventContext> db)
    {
        _db = db.DbContext;
    }

    public async Task<IEnumerable<Department>> GetDepartmentsAsync()
    {
        return await _db.Departments.ToListAsync();
    }

    public async Task<Department?> GetDepartmentAsync(int id)
    {
        var department = await _db.Departments
            .FirstOrDefaultAsync(d => d.Id == id);

        if (department != null)
        {
            return department;
        }

        return null;
    }

    public async Task<int> AddDepartmentAsync(string number, string name)
    {
        var department = new Department()
        {
            Name = name,
            Number = number
        };

        var newDepartment = await _db.Departments.AddAsync(department);
        await _db.SaveChangesAsync();

        return newDepartment.Entity.Id;
    }

    public async Task<int?> UpdateDepartmentAsync(int id, string number, string name)
    {
        var department = await _db.Departments
            .FirstOrDefaultAsync(d => d.Id == id);

        if (department == null)
        {
            return null;
        }

        department.Name = name;
        department.Number = number;

        var newDepartment = _db.Departments.Update(department);
        await _db.SaveChangesAsync();

        return newDepartment.Entity.Id;
    }

    public async Task<int?> RemoveDepartmentAsync(int id)
    {
        var department = await _db.Departments
            .FirstOrDefaultAsync(d => d.Id == id);

        if (department == null)
        {
            return null;
        }

        var removeEntity = _db.Departments.Remove(department);
        await _db.SaveChangesAsync();

        return removeEntity.Entity.Id;
    }
}