using MailLib.Models;

namespace MailLib.Interfaces;

public interface ISingleEmailOptions
{
    EmailAddressModel To { get; }

    string Subject { get; }

    string Body { get; }

    bool IsBodyHtml { get; }
}