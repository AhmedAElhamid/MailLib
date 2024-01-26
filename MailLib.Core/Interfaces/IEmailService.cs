using MailLib.Models;

namespace MailLib.Interfaces;

public interface IEmailService
{
    Task SendEmail(SingleEmailOptions options);

    Task SendEmail(SingleEmailToMultipleRecipientsOptions options);
    Task SendEmail(UserSpecificEmailBodyToMultipleRecipientsOptions options);
}