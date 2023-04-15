﻿namespace Application.Catalog.Services.Interfaces;

public interface ISpeakerService
{
    public Task<IEnumerable<SpeakerDto>> GetSpeakersAsync();
    public Task<SpeakerDto?> GetSpeakerAsync(int id);

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