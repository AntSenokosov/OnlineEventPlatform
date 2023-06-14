namespace Domain.Templates;

public class MailTemplate
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Html { get; set; }
    public string? Css { get; set; }
    public bool DefaultTemplate { get; set; }
    public bool WelcomeTemplate { get; set; }
}