using Microsoft.AspNetCore.Http;

namespace Application.Catalog.Services.Interfaces;

public interface IOnlineEventService
{
    public Task<IEnumerable<OnlineEventDto>> GetOnlineEventsAsync();
    public Task<OnlineEventDto?> GetOnlineEventAsync(int id);

    public Task<int> AddOnlineEventAsync(
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
        string? password);

    public Task<int?> UpdateOnlineEventAsync(
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
        string? password);

    public Task<int?> RemoveOnlineEventAsync(int id);
}