using Domain.Catalog.Entities;

namespace Application.Catalog.Repositories.Interfaces;

public interface IMeetingPlatformRepository
{
    public Task<IEnumerable<MeetingPlatform>> GetMeetingPlatforms();
}