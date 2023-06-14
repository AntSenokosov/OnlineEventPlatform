using Domain.Identity.Entities;

namespace Application.SendMail.Services.Interfaces;

public interface ISendMailService
{
    public Task<bool> SendMail(int eventId, string subject, string message);
    public Task<bool> SendOneMail(int eventId, string email);
    public Task<bool> SendInvitation(User user);
    public Task<bool> SendRecoveryPassword(User user, string password);
}