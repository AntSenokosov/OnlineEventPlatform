using Application.Catalog;
using Application.UserEvents.Repositories.Interfaces;
using Application.UserEvents.Services.Interfaces;
using AutoMapper;
using Infrastructure.Database;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.UserEvents.Services;

public class UserEventService : BaseDataService<OnlineEventContext>, IUserEventService
{
    private readonly IUserEventRepository _eventRepository;
    private readonly IMapper _mapper;

    public UserEventService(
        IDbContextWrapper<OnlineEventContext> dbContextWrapper,
        ILogger<BaseDataService<OnlineEventContext>> logger,
        IUserEventRepository eventRepository,
        IMapper mapper)
        : base(dbContextWrapper, logger)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OnlineEventDto>?> GetEvents()
    {
        return await ExecuteSafeAsync(async () =>
        {
            var events = await _eventRepository.GetEvents();

            if (events == null)
            {
                return null;
            }

            var eventsDto = events.Select(e => new OnlineEventDto()
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                Date = e.Date,
                AboutEvent = e.AboutEvent
            });

            return eventsDto;
        });
    }

    public async Task<int?> AddEvent(int eventId)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var userEvent = await _eventRepository.AddEvent(eventId);

            return _mapper.Map<int?>(userEvent);
        });
    }

    public async Task<int?> DeleteEvent(int eventId)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var userEvent = await _eventRepository.DeleteEvent(eventId);

            return _mapper.Map<int?>(userEvent);
        });
    }

    public async Task<bool> Check(int eventId)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var user = await _eventRepository.Check(eventId);

            return _mapper.Map<bool>(user);
        });
    }
}