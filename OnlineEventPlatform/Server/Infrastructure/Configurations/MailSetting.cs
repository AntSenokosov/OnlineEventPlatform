namespace Infrastructure.Configurations;

public class MailSetting
{
    public MailSetting()
    {
        DisplayName = "Khai";
        From = "a.y.senokosov@student.khai.edu";
        UserName = "a.y.senokosov@student.khai.edu";
        Password = "fjswcfhrmfkgiugl";
        Host = "smtp.gmail.com";
        Port = 587;
        UseSSL = false;
        UseStartTls = true;
    }

    public string? DisplayName { get; set; }
    public string? From { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? Host { get; set; }
    public int Port { get; set; }
    public bool UseSSL { get; set; }
    public bool UseStartTls { get; set; }
}