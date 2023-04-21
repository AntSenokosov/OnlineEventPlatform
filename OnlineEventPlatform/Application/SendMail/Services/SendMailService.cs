using Application.SendMail.Services.Interfaces;
using Domain.Catalog.Entities;
using Infrastructure.Database;
using Infrastructure.Exceptions;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using MimeKit;

namespace Application.SendMail.Services;

public class SendMailService : ISendMailService
{
    private readonly OnlineEventContext _context;
    private readonly string _fromEmail = "a.y.senokosov@student.khai.edu";
    private readonly string _fromPassword = "ankrirqczzsydytt";

    public SendMailService(OnlineEventContext context)
    {
        _context = context;
    }

    public async Task SendMail(int eventId, string subject, string message)
    {
        var onlineEvent = await _context.OnlineEvents
            .FirstOrDefaultAsync(o => o.Id == eventId);

        if (onlineEvent == null)
        {
            throw new ServiceException("Event not found");
        }

        var speakersEvent = await _context.SpeakerEvents
            .Where(s => s.OnlineEventId == onlineEvent.Id)
            .ToListAsync();

        var speakers = new List<Speaker>();

        foreach (var speakerEvent in speakersEvent)
        {
            var speaker = await _context.Speakers
                .FirstOrDefaultAsync(s => s.Id == speakerEvent.SpeakerId);

            if (speaker == null)
            {
                throw new ServiceException("Speaker not found");
            }

            speakers.Add(speaker);
        }

        var messageMail = new MimeMessage();
        messageMail.From.Add(new MailboxAddress("name_from", _fromEmail));

        var listAddressTo = new List<MailboxAddress>();

        var users = await _context.Users
            .ToListAsync();

        foreach (var user in users)
        {
            listAddressTo.Add(new MailboxAddress("name_to", user.Email));
        }

        messageMail.To.AddRange(listAddressTo);
        messageMail.Subject = subject;

        messageMail.Body = new TextPart("html")
        {
            Text = "<html>" +
                   "<head>" +
                   $"<title>{subject}</title>" +
                   "</head>" +
                   "<body>" +
                   $"<p>{message}</p>" +
                   "<hr />" +
                   "<h2>Speakers:</h2>" +
                   "<ul>" +
                   string.Join("", speakers.Select(s =>
                   {
                       return "<li>" +
                              $"<b>: {s.FirstName} {s.LastName}</b> - {s.Position}" +
                              "</li>";
                   })) +
                   "</ul>" +
                   "<hr />" +
                   "</body>" +
                   "</html>"
        };

        using var client = new SmtpClient();
        await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_fromEmail, _fromPassword);
        await client.SendAsync(messageMail);
        await client.DisconnectAsync(true);
    }
}