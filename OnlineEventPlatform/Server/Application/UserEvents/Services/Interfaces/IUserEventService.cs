using Application.Catalog;

namespace Application.UserEvents.Services.Interfaces;

public interface IUserEventService
{
    public Task<IEnumerable<OnlineEventDto>?> GetEvents();
    public Task<int?> AddEvent(int eventId);
    public Task<int?> DeleteEvent(int eventId);
    public Task<bool> Check(int eventId);
}