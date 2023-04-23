namespace Application.SendMail.Services.Interfaces;

public interface ISendMailService
{
    public Task<bool> SendMail(int eventId, string subject, string message);
}