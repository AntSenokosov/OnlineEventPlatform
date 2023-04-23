using Application.SendMail.Services.Interfaces;
using Infrastructure.Configurations;
using Infrastructure.Database;
using Infrastructure.Exceptions;
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
            var onlineEvent = await _context.OnlineEvents
                .FirstOrDefaultAsync(o => o.Id == eventId);

            if (onlineEvent == null)
            {
                throw new ServiceException("Event not found");
            }

            var messageMail = new MimeMessage();
            messageMail.From.Add(new MailboxAddress(_mailSetting.DisplayName, _mailSetting.From));
            messageMail.Sender = new MailboxAddress(_mailSetting.DisplayName, _mailSetting.From);

            messageMail.To.Add(MailboxAddress.Parse("anton.senokosov2001@gmail.com"));

            var body = new BodyBuilder();
            messageMail.Subject = subject;
            body.HtmlBody = GetTemplate(message);
            messageMail.Body = body.ToMessageBody();

            using var smtp = new SmtpClient();
            if (_mailSetting.UseSSL)
            {
                await smtp.ConnectAsync(_mailSetting.Host, _mailSetting.Port, SecureSocketOptions.SslOnConnect);
            }
            else if (_mailSetting.UseStartTls)
            {
                await smtp.ConnectAsync(_mailSetting.Host, _mailSetting.Port, SecureSocketOptions.StartTls);
            }

            await smtp.AuthenticateAsync(_mailSetting.UserName, _mailSetting.Password);
            await smtp.SendAsync(messageMail);
            await smtp.DisconnectAsync(true);

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    private string GetTemplate(string message)
    {
        var result = "<body style=\"background-color: #f4f4f4; margin: 0 !important; padding: 0 !important;\">" +
    @"<div style=""display: none; font-size: 1px; color: #fefefe; line-height: 1px; font-family: 'Lato', Helvetica, Arial, sans-serif; max-height: 0px; max-width: 0px; opacity: 0; overflow: hidden;"">@Model?.Name We're thrilled to have you here! Get ready to dive into your new account. </div>" +
    "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">" +
        "<tr>" +
            "<td bgcolor=\"#FFA73B\" align=\"center\">" +
                "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">" +
                    "<tr>" +
                        "<td align=\"center\" valign=\"top\" style=\"padding: 40px 10px 40px 10px;\"></td>" +
                    "</tr>" +
                "</table>" +
            "</td>" +
        "</tr>" +
        "<tr>" +
            "<td bgcolor=\"#FFA73B\" align=\"center\" style=\"padding: 0px 10px 0px 10px;\">" +
                "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">" +
                    "<tr>" +
                        "<td bgcolor=\"#ffffff\" align=\"center\" valign=\"top\" style=\"padding: 40px 20px 20px 20px; border-radius: 4px 4px 0px 0px; color: #111111; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 48px; font-weight: 400; letter-spacing: 4px; line-height: 48px;\">" +
                            "<h1 style=\"font-size: 48px; font-weight: 400; margin: 2;\">Welcome @Model?.Name!</h1> <img src=\" https://img.icons8.com/clouds/100/000000/handshake.png\" width=\"125\" height=\"120\" style=\"display: block; border: 0px;\" />" +
                        "</td>" +
                    "</tr>" +
                "</table>" +
            "</td>" +
        "</tr>" +
        "<tr>" +
            "<td bgcolor=\"#f4f4f4\" align=\"center\" style=\"padding: 0px 10px 0px 10px;\">" +
                "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">" +
                    "<tr>" +
                       " <td bgcolor=\"#ffffff\" align=\"left\" style=\"padding: 20px 30px 40px 30px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;\">" +
                            "<p style=\"margin: 0;\">We're excited to have you get started at the MailKit Demo Platform. First, you need to confirm your account. Just press the button below.</p>" +
                        "</td>" +
                    "</tr>" +
                    "<tr>" +
                        "<td bgcolor=\"#ffffff\" align=\"left\">" +
                            "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" +
                                "<tr>" +
                                   "<td bgcolor=\"#ffffff\" align=\"center\" style=\"padding: 20px 30px 60px 30px;\">" +
                                        "<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" +
                                            "<tr>" +
                                                "<td align=\"center\" style=\"border-radius: 3px;\" bgcolor=\"#FFA73B\"><a href=\"#\" target=\"_blank\" style=\"font-size: 20px; font-family: Helvetica, Arial, sans-serif; color: #ffffff; text-decoration: none; color: #ffffff; text-decoration: none; padding: 15px 25px; border-radius: 2px; border: 1px solid #FFA73B; display: inline-block;\">Confirm Account</a></td>" +
                                            "</tr>" +
                                        "</table>" +
                                    "</td>" +
                                "</tr>" +
                            "</table>" +
                        "</td>" +
                    "<tr>" +
                        "<td bgcolor=\"#ffffff\" align=\"left\" style=\"padding: 0px 30px 0px 30px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;\">" +
                            "<p style=\"margin: 0;\">If that doesn't work, copy and paste the following link in your browser:</p>" +
                        "</td>" +
                    "<tr>" +
                        "<td bgcolor=\"#ffffff\" align=\"left\" style=\"padding: 20px 30px 20px 30px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;\">" +
                            "<p style=\"margin: 0;\"><a href=\"#\" target=\"_blank\" style=\"color: #FFA73B;\">Some URL to activate the account</a></p>" +
                        "</td>" +
                    "</tr>" +
                    "<tr>" +
                        "<td bgcolor=\"#ffffff\" align=\"left\" style=\"padding: 0px 30px 20px 30px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;\">" +
                            "<p style=\"margin: 0;\">If you have any questions, just reply to this email—we're always happy to help out.</p>" +
                        "</td>" +
                    "</tr>" +
                    "<tr>" +
                        "<td bgcolor=\"#ffffff\" align=\"left\" style=\"padding: 0px 30px 40px 30px; border-radius: 0px 0px 4px 4px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;\">" +
                            "<p style=\"margin: 0;\">Cheers,<br>BBB Team</p>" +
                        "</td>" +
                    "</tr>" +
                "</table>" +
            "</td>" +
        "</tr>" +
        "<tr>" +
            "<td bgcolor=\"#f4f4f4\" align=\"center\" style=\"padding: 30px 10px 0px 10px;\">" +
                "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\">" +
                    "<tr>" +
                        "<td bgcolor=\"#FFECD1\" align=\"center\" style=\"padding: 30px 30px 30px 30px; border-radius: 4px 4px 4px 4px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;\">" +
                            "<h2 style=\"font-size: 20px; font-weight: 400; color: #111111; margin: 0;\">Need more help?</h2>" +
                            "<p style=\"margin: 0;\"><a href=\"#\" target=\"_blank\" style=\"color: #FFA73B;\">We&rsquo;re here to help you out</a></p>" +
                        "</td>" +
                    "</tr>" +
                "</table>" +
            "</td>" +
        "</tr>" +
    "</table>" +
"</body>";

        return result;
    }
}