using Application.Catalog.Services.Interfaces;
using AutoMapper;
using Domain.Catalog.Repositories.Interfaces;
using Infrastructure.Database;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Catalog.Services;

public class OnlineEventService : BaseDataService<OnlineEventContext>, IOnlineEventService
{
    private readonly IMapper _mapper;
    private readonly IOnlineEventRepository _eventRepository;

    public OnlineEventService(
        IDbContextWrapper<OnlineEventContext> dbContextWrapper,
        ILogger<BaseDataService<OnlineEventContext>> logger,
        IMapper mapper,
        IOnlineEventRepository eventRepository)
        : base(dbContextWrapper, logger)
    {
        _mapper = mapper;
        _eventRepository = eventRepository;
    }

    public async Task<IEnumerable<OnlineEventDto>> GetOnlineEventsAsync()
    {
        return await ExecuteSafeAsync(async () =>
        {
            var events = await _eventRepository.GetOnlineEventsAsync();
            return events.Select(e => _mapper.Map<OnlineEventDto>(e));
        });
    }

    public async Task<OnlineEventDto?> GetOnlineEventAsync(int id)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var onlineEvent = await _eventRepository.GetOnlineEventAsync(id);
            return _mapper.Map<OnlineEventDto?>(onlineEvent);
        });
    }

    public async Task<int> AddOnlineEventAsync(string name, string description, DateTime dateTime, string aboutEvent)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var onlineEvent = await _eventRepository
                .AddOnlineEventAsync(name, description, dateTime, aboutEvent);
            return _mapper.Map<int>(onlineEvent);
        });
    }

    public async Task<int?> UpdateOnlineEventAsync(int id, string name, string description, DateTime dateTime, string aboutEvent)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var onlineEvent = await _eventRepository
                .UpdateOnlineEventAsync(id, name, description, dateTime, aboutEvent);
            return _mapper.Map<int?>(onlineEvent);
        });
    }

    public async Task<int?> RemoveOnlineEventAsync(int id)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var onlineEvent = await _eventRepository.RemoveOnlineEventAsync(id);
            return _mapper.Map<int?>(onlineEvent);
        });
    }
}