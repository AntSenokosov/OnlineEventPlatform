using Application.Catalog.Repositories.Interfaces;
using Application.Catalog.Services.Interfaces;
using AutoMapper;
using Domain.Catalog.Entities;
using Infrastructure.Database;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Catalog.Services;

public class EventTypeService : BaseDataService<OnlineEventContext>, IEventTypeService
{
    private readonly IEventTypeRepository _typeRepository;
    private readonly IMapper _mapper;
    public EventTypeService(IDbContextWrapper<OnlineEventContext> dbContextWrapper, ILogger<BaseDataService<OnlineEventContext>> logger, IEventTypeRepository typeRepository, IMapper mapper)
        : base(dbContextWrapper, logger)
    {
        _typeRepository = typeRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EventTypeDto>> GetTypes()
    {
        return await ExecuteSafeAsync(async () =>
        {
            var types = await _typeRepository.GetTypes();

            return types.Select(t => new EventTypeDto()
            {
                Id = t.Id,
                Name = t.Name
            });
        });
    }
}