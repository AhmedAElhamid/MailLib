using MailLib.Models;

namespace MailLib.Interfaces;

public interface ISingleEmailToMultipleRecipientsOptions
{
    List<EmailAddressModel> To { get; }

    string Subject { get; }

    string Body { get; }

    bool IsHtmlBody { get; }
}