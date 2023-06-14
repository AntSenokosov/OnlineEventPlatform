using Application.Catalog.Repositories.Interfaces;
using Application.Catalog.Services.Interfaces;
using AutoMapper;
using Infrastructure.Database;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Catalog.Services;

public class MeetingPlatformService : BaseDataService<OnlineEventContext>, IMeetingPlatformService
{
    private readonly IMeetingPlatformRepository _platformRepository;
    private readonly IMapper _mapper;
    public MeetingPlatformService(
        IDbContextWrapper<OnlineEventContext> dbContextWrapper,
        ILogger<BaseDataService<OnlineEventContext>> logger,
        IMeetingPlatformRepository platformRepository,
        IMapper mapper)
        : base(dbContextWrapper, logger)
    {
        _platformRepository = platformRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MeetingPlatformDto>> GetPlatforms()
    {
        return await ExecuteSafeAsync(async () =>
        {
            var platforms = await _platformRepository.GetMeetingPlatforms();

            return platforms.Select(p => new MeetingPlatformDto()
            {
                Id = p.Id,
                Name = p.Name
            });
        });
    }
}