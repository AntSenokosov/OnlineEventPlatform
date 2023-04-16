using Application.Catalog.Services.Interfaces;
using AutoMapper;
using Application.Catalog.Repositories.Interfaces;
using Infrastructure.Database;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Catalog.Services;

public class SpeakerService : BaseDataService<OnlineEventContext>, ISpeakerService
{
    private readonly IMapper _mapper;
    private readonly ISpeakerRepository _speakerRepository;

    public SpeakerService(
        IDbContextWrapper<OnlineEventContext> dbContextWrapper,
        ILogger<BaseDataService<OnlineEventContext>> logger,
        IMapper mapper,
        ISpeakerRepository speakerRepository)
        : base(dbContextWrapper, logger)
    {
        _mapper = mapper;
        _speakerRepository = speakerRepository;
    }

    public async Task<IEnumerable<SpeakerDto>> GetSpeakersAsync()
    {
        return await ExecuteSafeAsync(async () =>
        {
            var speakers = await _speakerRepository.GetSpeakersAsync();

            var speakersDto = speakers.Select(s => new SpeakerDto()
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Description = s.Description,
                DepartmentId = s.DepartmentId,
                PositionId = s.PositionId
            });

            return speakersDto;
        });
    }

    public async Task<SpeakerDto?> GetSpeakerAsync(int id)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var speaker = await _speakerRepository.GetSpeakerAsync(id);

            if (speaker == null)
            {
                return null;
            }

            var speakerDto = new SpeakerDto()
            {
                Id = speaker.Id,
                FirstName = speaker.FirstName,
                LastName = speaker.LastName,
                PositionId = speaker.PositionId,
                DepartmentId = speaker.DepartmentId,
                Description = speaker.Description
            };

            return speakerDto;
        });
    }

    public async Task<int> AddSpeakerAsync(
        string firstName,
        string lastName,
        int departmentId,
        int positionId,
        string description)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var speaker = await _speakerRepository
                .AddSpeakerAsync(firstName, lastName, departmentId, positionId, description);
            return _mapper.Map<int>(speaker);
        });
    }

    public async Task<int?> UpdateSpeakerAsync(
        int id,
        string firstName,
        string lastName,
        int departmentId,
        int positionId,
        string description)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var speaker = await _speakerRepository
                .UpdateSpeakerAsync(id, firstName, lastName, departmentId, positionId, description);
            return _mapper.Map<int?>(speaker);
        });
    }

    public async Task<int?> RemoveSpeakerAsync(int id)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var speaker = await _speakerRepository.RemoveSpeakerAsync(id);
            return _mapper.Map<int?>(speaker);
        });
    }
}