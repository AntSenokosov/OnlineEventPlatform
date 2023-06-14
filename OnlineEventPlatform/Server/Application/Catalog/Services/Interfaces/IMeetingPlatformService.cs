namespace Application.Catalog.Services.Interfaces;

public interface IMeetingPlatformService
{
    public Task<IEnumerable<MeetingPlatformDto>> GetPlatforms();
}