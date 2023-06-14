using Application.Catalog.Services.Interfaces;
using AutoMapper;
using Application.Catalog.Repositories.Interfaces;
using Infrastructure.Database;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;
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
        return await ExecuteSafeAsync(async () => await _eventRepository.GetOnlineEventsAsync());
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
                Date = onlineEvent.Date,
                Time = onlineEvent.Time,
                AboutEvent = onlineEvent.AboutEvent
            };

            return eventDto;
        });
    }

    public async Task<int> AddOnlineEventAsync(
        int type,
        string name,
        string description,
        DateTime date,
        TimeSpan time,
        string aboutEvent,
        IFormFile? photo,
        IEnumerable<int>? speakers,
        int platform,
        string? link,
        string? meetingId,
        string? password)
    {
        return await ExecuteSafeAsync(async () =>
        {
            if (photo != null)
            {
                using var memoryStream = new MemoryStream();
                await photo.CopyToAsync(memoryStream);
                memoryStream.ToArray();
            }

            var onlineEvent = await _eventRepository
                .AddOnlineEventAsync(
                    type,
                    name,
                    description,
                    date,
                    time,
                    aboutEvent,
                    photo,
                    speakers,
                    platform,
                    link,
                    meetingId,
                    password);
            return _mapper.Map<int>(onlineEvent);
        });
    }

    public async Task<int?> UpdateOnlineEventAsync(
        int id,
        int type,
        string name,
        string description,
        DateTime date,
        TimeSpan time,
        string aboutEvent,
        IFormFile? photo,
        IEnumerable<int>? speakers,
        int platform,
        string? link,
        string? meetingId,
        string? password)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var onlineEvent = await _eventRepository
                .UpdateOnlineEventAsync(
                    id,
                    type,
                    name,
                    description,
                    date,
                    time,
                    aboutEvent,
                    photo,
                    speakers,
                    platform,
                    link,
                    meetingId,
                    password);
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