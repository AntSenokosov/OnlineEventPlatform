using Application.Catalog.Services.Interfaces;
using AutoMapper;
using Application.Catalog.Repositories.Interfaces;
using Infrastructure.Database;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Catalog.Services;

public class PositionService : BaseDataService<OnlineEventContext>, IPositionService
{
    private readonly IMapper _mapper;
    private readonly IPositionRepository _positionRepository;

    public PositionService(
        IDbContextWrapper<OnlineEventContext> dbContextWrapper,
        ILogger<BaseDataService<OnlineEventContext>> logger,
        IMapper mapper,
        IPositionRepository positionRepository)
        : base(dbContextWrapper, logger)
    {
        _mapper = mapper;
        _positionRepository = positionRepository;
    }

    public async Task<IEnumerable<PositionDto>> GetPositionsAsync()
    {
        return await ExecuteSafeAsync(async () =>
        {
            var positions = await _positionRepository.GetPositionsAsync();

            var positionsDto = positions.Select(p => new PositionDto()
            {
                Id = p.Id,
                Name = p.Name
            });

            return positionsDto;
        });
    }

    public async Task<PositionDto?> GetPositionAsync(int id)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var position = await _positionRepository.GetPositionAsync(id);

            if (position == null)
            {
                return null;
            }

            var positionDto = new PositionDto()
            {
                Id = position.Id,
                Name = position.Name
            };

            return positionDto;
        });
    }

    public async Task<int> AddPositionAsync(string name)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var position = await _positionRepository.AddPositionAsync(name);
            return _mapper.Map<int>(position);
        });
    }

    public async Task<int?> UpdatePositionAsync(int id, string name)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var position = await _positionRepository.UpdatePositionAsync(id, name);
            return _mapper.Map<int?>(position);
        });
    }

    public async Task<int?> RemovePositionAsync(int id)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var position = await _positionRepository.RemovePositionAsync(id);
            return _mapper.Map<int?>(position);
        });
    }
}