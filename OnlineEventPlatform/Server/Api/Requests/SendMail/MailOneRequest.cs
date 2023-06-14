namespace Api.Requests.SendMail;

public class MailOneRequest
{
    public int EventId { get; set; }
    public string Email { get; set; } = null!;
}