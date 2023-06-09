﻿using Domain.Catalog.Entities;
using Application.Catalog.Repositories.Interfaces;
using Infrastructure.Database;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Catalog.Repositories;

public class SpeakerRepository : ISpeakerRepository
{
    private readonly OnlineEventContext _db;

    public SpeakerRepository(IDbContextWrapper<OnlineEventContext> db)
    {
        _db = db.DbContext;
    }

    public async Task<IEnumerable<Speaker>> GetSpeakersAsync()
    {
        return await _db.Speakers.ToListAsync();
    }

    public async Task<Speaker?> GetSpeakerAsync(int id)
    {
        var speaker = await _db.Speakers.FirstOrDefaultAsync(s => s.Id == id);

        if (speaker == null)
        {
            return null;
        }

        return speaker;
    }

    public async Task<int> AddSpeakerAsync(
        string firstName,
        string lastName,
        string? position,
        string? shortDescription,
        string? longDescription)
    {
        var speaker = new Speaker()
        {
            FirstName = firstName,
            LastName = lastName,
            Position = position,
            ShortDescription = shortDescription,
            LongDescription = longDescription
        };

        var newSpeaker = await _db.Speakers.AddAsync(speaker);
        await _db.SaveChangesAsync();

        return newSpeaker.Entity.Id;
    }

    public async Task<int?> UpdateSpeakerAsync(
        int id,
        string firstName,
        string lastName,
        string? position,
        string? shortDescription,
        string? longDescription)
    {
        var speaker = await _db.Speakers.FirstOrDefaultAsync(s => s.Id == id);

        if (speaker == null)
        {
            return null;
        }

        speaker.FirstName = firstName;
        speaker.LastName = lastName;
        speaker.Position = position;
        speaker.ShortDescription = shortDescription;
        speaker.LongDescription = longDescription;

        var newSpeaker = _db.Speakers.Update(speaker);
        await _db.SaveChangesAsync();

        return newSpeaker.Entity.Id;
    }

    public async Task<int?> RemoveSpeakerAsync(int id)
    {
        var speaker = await _db.Speakers.FirstOrDefaultAsync(s => s.Id == id);

        if (speaker == null)
        {
            return null;
        }

        var removeSpeaker = _db.Speakers.Remove(speaker);
        await _db.SaveChangesAsync();

        return removeSpeaker.Entity.Id;
    }
}