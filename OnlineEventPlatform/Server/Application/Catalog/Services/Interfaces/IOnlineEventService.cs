namespace Application.Catalog.Services.Interfaces;

public interface IOnlineEventService
{
    public Task<IEnumerable<OnlineEventDto>> GetOnlineEventsAsync();
    public Task<OnlineEventDto?> GetOnlineEventAsync(int id);

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