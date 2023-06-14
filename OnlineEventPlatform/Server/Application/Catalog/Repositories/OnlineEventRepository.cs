using System.Net;
using Domain.Catalog.Entities;
using Application.Catalog.Repositories.Interfaces;
using Infrastructure.Database;
using Infrastructure.Helpers;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Catalog.Repositories;

public class OnlineEventRepository : IOnlineEventRepository
{
    private readonly OnlineEventContext _db;

    public OnlineEventRepository(IDbContextWrapper<OnlineEventContext> db)
    {
        _db = db.DbContext;
    }

    public async Task<IEnumerable<OnlineEventDto>> GetOnlineEventsAsync()
    {
        var onlineEvents = await _db.OnlineEvents
            .Include(e => e.Type)
            .Include(e => e.Speakers)
            .ThenInclude(es => es.Speaker)
            .Include(e => e.EventPlatform)
            .ThenInclude(ep => ep.MeetingPlatform)
            .ToListAsync();

        var onlineEventDtos = onlineEvents.Select(e => new OnlineEventDto
        {
            Id = e.Id,
            Type = e.Type.Id,
            Name = e.Name,
            Description = e.Description,
            Date = e.Date,
            Time = e.Time,
            AboutEvent = e.AboutEvent,
            Photo = null, // Assign the appropriate value based on your requirements
            Speakers = e.Speakers.Select(es => es.Speaker.Id),
            Platform = e.EventPlatform.MeetingPlatform.Id,
            Link = e.EventPlatform.Link,
            MeetingId = e.EventPlatform.LinkId,
            Password = e.EventPlatform.Password
        });

        return onlineEventDtos;
    }

    public async Task<OnlineEvent?> GetOnlineEventAsync(int id)
    {
        var onlineEvent = await _db.OnlineEvents.FirstOrDefaultAsync(e => e.Id == id);

        if (onlineEvent == null)
        {
            return null;
        }

        return onlineEvent;
    }

    public async Task<int> AddOnlineEventAsync(
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
        string? password)
    {
        var eventSpeakers = new List<EventSpeaker>();

        foreach (var speakerId in speakers!)
        {
            var speaker =
                await _db.EventSpeakers.FirstOrDefaultAsync(m => m.SpeakerId == speakerId);

            if (speaker == null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { Error = "Error while reading the member" });
            }

            eventSpeakers.Add(speaker);
        }

        var onlineEvent = new OnlineEvent()
        {
            Name = name,
            TypeId = type,
            Description = description,
            Date = date,
            Time = time,
            AboutEvent = aboutEvent,
            Speakers = eventSpeakers
        };

        if (photo != null)
        {
            using (var memoryStream = new MemoryStream())
            {
                await photo.CopyToAsync(memoryStream);
                onlineEvent.Photo = memoryStream.ToArray();
            }
        }

        await _db.OnlineEvents.AddAsync(onlineEvent);
        await _db.SaveChangesAsync();

        var eventPlatform = new EventPlatform()
        {
            EventId = onlineEvent.Id,
            PlatformId = platform,
            LinkId = meetingId,
            Password = password
        };

        await _db.EventPlatforms.AddAsync(eventPlatform);
        await _db.SaveChangesAsync();

        return onlineEvent.Id;
    }

    public async Task<int?> UpdateOnlineEventAsync(
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
        string? password)
    {
        var onlineEvent = await _db.OnlineEvents
        .Include(e => e.Speakers)
        .FirstOrDefaultAsync(e => e.Id == id);

        if (onlineEvent == null)
        {
            throw new RestException(
                HttpStatusCode.BadRequest,
                new
                {
                    Message = "Event not found"
                });
        }

        onlineEvent.TypeId = type;
        onlineEvent.Name = name;
        onlineEvent.Description = description;
        onlineEvent.AboutEvent = aboutEvent;
        onlineEvent.Date = date;
        onlineEvent.Time = time;

        if (photo != null)
        {
            using (var memoryStream = new MemoryStream())
            {
                await photo.CopyToAsync(memoryStream);
                onlineEvent.Photo = memoryStream.ToArray();
            }
        }

        var existingSpeakerIds = onlineEvent.Speakers.Select(s => s.SpeakerId).ToList();
        var speakersToAdd = speakers?.Except(existingSpeakerIds) ?? Enumerable.Empty<int>();
        var speakersToRemove = existingSpeakerIds.Except(speakers ?? Enumerable.Empty<int>());

        foreach (var speakerId in speakersToAdd)
        {
            var speaker = await _db.Speakers.FindAsync(speakerId);
            if (speaker != null)
            {
                onlineEvent.Speakers.Add(new EventSpeaker
                {
                    Speaker = speaker
                });
            }
        }

        foreach (var speakerId in speakersToRemove)
        {
            var speakerToRemove = onlineEvent.Speakers.FirstOrDefault(s => s.SpeakerId == speakerId);
            if (speakerToRemove != null)
            {
                onlineEvent.Speakers.Remove(speakerToRemove);
            }
        }

        var eventPlatform = await _db.EventPlatforms
            .FirstOrDefaultAsync(p => p.EventId == onlineEvent.Id);

        if (eventPlatform != null)
        {
            eventPlatform.PlatformId = platform;
            eventPlatform.Link = link;
            eventPlatform.LinkId = meetingId;
            eventPlatform.Password = password;

            _db.EventPlatforms.Update(eventPlatform);
        }

        _db.OnlineEvents.Update(onlineEvent);

        await _db.SaveChangesAsync();

        return onlineEvent.Id;
    }

    public async Task<int?> RemoveOnlineEventAsync(int id)
    {
        var onlineEvent = await _db.OnlineEvents.FirstOrDefaultAsync(e => e.Id == id);

        if (onlineEvent == null)
        {
            return null;
        }

        var removeEvent = _db.OnlineEvents.Remove(onlineEvent);
        await _db.SaveChangesAsync();

        return removeEvent.Entity.Id;
    }
}