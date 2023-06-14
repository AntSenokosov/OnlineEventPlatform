using Application.SendMail.Services.Interfaces;
using Domain.Identity.Entities;
using Infrastructure.Configurations;
using Infrastructure.Database;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Application.SendMail.Services;

public class SendMailService : ISendMailService
{
    private readonly OnlineEventContext _context;
    private readonly MailSetting _mailSetting;

    public SendMailService(OnlineEventContext context, IOptions<MailSetting> mailSetting)
    {
        _context = context;
        _mailSetting = mailSetting.Value;
    }

    public async Task<bool> SendMail(int eventId, string subject, string message)
    {
        try
        {
            /*
            var onlineEvent = await _context.OnlineEvents.FirstOrDefaultAsync(o => o.Id == eventId);

            if (onlineEvent == null)
            {
                throw new ServiceException("Event not found");
            }
            */

            var messageMail = new MimeMessage();
            messageMail.From.Add(new MailboxAddress(_mailSetting.DisplayName, _mailSetting.From));
            messageMail.To.Add(MailboxAddress.Parse("anton.senokosov2001@gmail.com"));
            messageMail.Subject = subject;

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == 11);

            var body = new TextPart("html")
            {
                Text = GetTemplate()
            };

            messageMail.Body = body;

            using var client = new SmtpClient();
            await client.ConnectAsync(_mailSetting.Host, _mailSetting.Port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_mailSetting.UserName, _mailSetting.Password);
            await client.SendAsync(messageMail);
            await client.DisconnectAsync(true);

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> SendOneMail(int eventId, string email)
    {
        try
        {
            var messageMail = new MimeMessage();
            messageMail.From.Add(new MailboxAddress(_mailSetting.DisplayName, _mailSetting.From));
            messageMail.To.Add(MailboxAddress.Parse(email));
            messageMail.Subject = "subject";

            // var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == 11);
            var body = new TextPart("html")
            {
                Text = GetTemplate()
            };

            messageMail.Body = body;

            using var client = new SmtpClient();
            await client.ConnectAsync(_mailSetting.Host, _mailSetting.Port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_mailSetting.UserName, _mailSetting.Password);
            await client.SendAsync(messageMail);
            await client.DisconnectAsync(true);

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> SendInvitation(User user)
    {
        try
        {
            var messageMail = new MimeMessage();
            messageMail.From.Add(new MailboxAddress(_mailSetting.DisplayName, _mailSetting.From));
            messageMail.To.Add(MailboxAddress.Parse(user.Email));
            messageMail.Subject = "Welcome to Our Platform";

            var body = new TextPart("html")
            {
                Text = GetWelcomeTemplate(user)
            };

            messageMail.Body = body;

            using var client = new SmtpClient();
            await client.ConnectAsync(_mailSetting.Host, _mailSetting.Port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_mailSetting.UserName, _mailSetting.Password);
            await client.SendAsync(messageMail);
            await client.DisconnectAsync(true);

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> SendRecoveryPassword(User user, string password)
    {
        try
        {
            var messageMail = new MimeMessage();
            messageMail.From.Add(new MailboxAddress(_mailSetting.DisplayName, _mailSetting.From));
            messageMail.To.Add(MailboxAddress.Parse(user.Email));
            messageMail.Subject = "Відновлення паролю";

            var body = new TextPart("html")
            {
                Text = TemplateRecoveryPassword(user, password)
            };

            messageMail.Body = body;

            using var client = new SmtpClient();
            await client.ConnectAsync(_mailSetting.Host, _mailSetting.Port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_mailSetting.UserName, _mailSetting.Password);
            await client.SendAsync(messageMail);
            await client.DisconnectAsync(true);

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    private string TemplateRecoveryPassword(User user, string password)
    {
        var template = _context.MailTemplates.FirstOrDefault(t => t.Id == 5)!.Html;

        // var template = File.ReadAllText("../../../Infrastructure/Files/HtmlTemplate/WelcomeTemplate.html");
        template = template!.Replace("{UserName}", $"{user.FirstName} {user.LastName}");
        template = template.Replace("{Password}", password);

        return template;
    }

    private string GetWelcomeTemplate(User user)
    {
        var template = _context.MailTemplates.FirstOrDefault(t => t.DefaultTemplate == true)!.Html;
        var welcomeTemplate = _context.MailTemplates.FirstOrDefault(t => t.WelcomeTemplate == true);

        // var template = File.ReadAllText("../../../Infrastructure/Files/HtmlTemplate/WelcomeTemplate.html");
        var activationLink = "localhost:4200";

        template = template!.Replace("{Html}", $"{welcomeTemplate!.Html}")
            .Replace("{Css}", $"{welcomeTemplate.Css}")
            .Replace("{UserName}", $"{user.FirstName} {user.LastName}")
            .Replace("{ActivationLink}", activationLink);

        return template;
    }

    private string GetTemplate()
    {
        var template = _context.MailTemplates.FirstOrDefault(t => t.Id == 1)!.Html;

        return template!;
    }
}