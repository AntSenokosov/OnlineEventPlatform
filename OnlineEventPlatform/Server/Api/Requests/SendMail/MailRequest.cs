namespace Api.Requests.SendMail;

public class MailRequest
{
    public int EventId { get; set; }
    public string Subject { get; set; } = null!;
    public string Message { get; set; } = null!;
}