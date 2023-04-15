using Domain.Catalog.Entities;

namespace Domain.Catalog.Repositories.Interfaces;

public interface IOnlineEventRepository
{
    public Task<IEnumerable<OnlineEvent>> GetOnlineEventsAsync();
    public Task<OnlineEvent?> GetOnlineEventAsync(int id);

    public Task<int> AddOnlineEventAsync(
        string name,
        string description,
        DateTime dateTime,
        string aboutEvent);

    public Task<int?> UpdateOnlineEventAsync(
        int id,
        string name,
        string description,
        DateTime dateTime,
        string aboutEvent);

    public Task<int?> RemoveOnlineEventAsync(int id);
}