using Application.Catalog.Services.Interfaces;
using AutoMapper;
using Application.Catalog.Repositories.Interfaces;
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

            var eventsDto = events.Select(e => new OnlineEventDto()
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                DateAndTime = e.DateAndTime,
                AboutEvent = e.AboutEvent
            });

            return eventsDto;
        });
    }

    public async Task<OnlineEventDto?> GetOnlineEventAsync(int id)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var onlineEvent = await _eventRepository.GetOnlineEventAsync(id);

            if (onlineEvent == null)
            {
                return null;
            }

            var eventDto = new OnlineEventDto()
            {
                Id = onlineEvent.Id,
                Name = onlineEvent.Name,
                Description = onlineEvent.Description,
                DateAndTime = onlineEvent.DateAndTime,
                AboutEvent = onlineEvent.AboutEvent
            };

            return eventDto;
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