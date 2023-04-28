using Domain.Catalog.Entities;

namespace Application.Catalog.Repositories.Interfaces;

public interface ISpeakerRepository
{
    public Task<IEnumerable<Speaker>> GetSpeakersAsync();
    public Task<Speaker?> GetSpeakerAsync(int id);

    public Task<int> AddSpeakerAsync(
        string firstName,
        string lastName,
        int departmentId,
        int positionId,
        string description);

    public Task<int?> UpdateSpeakerAsync(
        int id,
        string firstName,
        string lastName,
        int departmentId,
        int positionId,
        string description);

    public Task<int?> RemoveSpeakerAsync(int id);
}