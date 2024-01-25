namespace MailLib.Models;

public class SmtpConfiguration(
    string smtpHost,
    int port,
    string from,
    string? smtpUser = default,
    string? smtpPassword = default)
{
    public string From { get; set; } = from ?? throw new ArgumentNullException(nameof(from));
    public string Host { get; set; } = smtpHost ?? throw new ArgumentNullException(nameof(smtpHost));

    public int Port { get; set; } = port > 0 ? port : throw new ArgumentOutOfRangeException(nameof(port));
    public string? User { get; set; } = smtpUser;
    public string? Password { get; set; } = smtpPassword;
}