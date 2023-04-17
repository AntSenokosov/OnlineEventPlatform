using Domain.Catalog.Entities;

namespace Application.UserEvents.Repositories.Interfaces;

public interface IUserEventRepository
{
    public Task<IEnumerable<OnlineEvent>?> GetEvents();
    public Task<int?> AddEvent(int eventId);
    public Task<int?> DeleteEvent(int eventId);
}