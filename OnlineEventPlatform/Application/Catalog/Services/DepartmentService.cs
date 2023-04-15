using Application.Catalog.Services.Interfaces;
using AutoMapper;
using Domain.Catalog.Repositories.Interfaces;
using Infrastructure.Database;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Catalog.Services;

public class DepartmentService : BaseDataService<OnlineEventContext>, IDepartmentService
{
    private readonly IMapper _mapper;
    private readonly IDepartmentRepository _departmentRepository;

    public DepartmentService(
        IDbContextWrapper<OnlineEventContext> dbContextWrapper,
        ILogger<BaseDataService<OnlineEventContext>> logger,
        IMapper mapper,
        IDepartmentRepository departmentRepository)
        : base(dbContextWrapper, logger)
    {
        _mapper = mapper;
        _departmentRepository = departmentRepository;
    }

    public async Task<IEnumerable<DepartmentDto>> GetDepartmentsAsync()
    {
        return await ExecuteSafeAsync(async () =>
        {
            var departments = await _departmentRepository.GetDepartmentsAsync();
            return departments.Select(d => _mapper.Map<DepartmentDto>(d));
        });
    }

    public async Task<DepartmentDto?> GetDepartmentAsync(int id)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var department = await _departmentRepository.GetDepartmentAsync(id);
            return _mapper.Map<DepartmentDto?>(department);
        });
    }

    public async Task<int> AddDepartmentAsync(string number, string name)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var department = await _departmentRepository.AddDepartmentAsync(number, name);
            return _mapper.Map<int>(department);
        });
    }

    public async Task<int?> UpdateDepartmentAsync(int id, string number, string name)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var department = await _departmentRepository.UpdateDepartmentAsync(id, number, name);
            return _mapper.Map<int?>(department);
        });
    }

    public async Task<int?> RemoveDepartmentAsync(int id)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var department = await _departmentRepository.RemoveDepartmentAsync(id);
            return _mapper.Map<int?>(department);
        });
    }
}