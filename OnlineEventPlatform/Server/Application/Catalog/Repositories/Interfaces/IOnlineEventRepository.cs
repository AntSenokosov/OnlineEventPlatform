using Domain.Catalog.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Catalog.Repositories.Interfaces;

public interface IOnlineEventRepository
{
    public Task<IEnumerable<OnlineEventDto>> GetOnlineEventsAsync();
    public Task<OnlineEvent?> GetOnlineEventAsync(int id);

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