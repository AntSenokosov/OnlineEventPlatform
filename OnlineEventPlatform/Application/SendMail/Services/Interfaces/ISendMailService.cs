namespace Application.SendMail.Services.Interfaces;

public interface ISendMailService
{
    public Task SendMail(int eventId, string subject, string message);
}