using System.Text.Json.Serialization;
using MailLib.Extensions;
using MailLib.Interfaces;

namespace MailLib.Models;

public class SingleEmailOptions : ISingleEmailOptions
{
    public EmailAddressModel To { get; private set; }

    public string Subject { get; private set; }

    public string Body { get; private set; }

    public MailResources MailResources { get; set; } = new();
    public bool IsBodyHtml { get; private set; }

    public string? PlainTextContent => !IsBodyHtml ? Body : null;

    public string? HtmlContent => !IsBodyHtml ? null : Body;

    public SingleEmailOptions(EmailAddressModel to, string subject, string textBody,
        MailResources? mailResources = default)
    {
        To = to;
        Subject = ValidationExtensions.NotEmptyOrWhiteSpace(subject, nameof(subject));
        Body = ValidationExtensions.NotEmptyOrWhiteSpace(textBody, nameof(textBody));
        IsBodyHtml = false;
        MailResources = mailResources ?? new MailResources();
    }

    [JsonConstructor]
    public SingleEmailOptions(
        EmailAddressModel to,
        string subject,
        string htmlBody,
        List<TemplatePlaceholder> placeholders,
        MailResources? mailResources = default
    )
    {
        var input = htmlBody.ReplacePlaceholders(placeholders);
        To = to;
        Subject = ValidationExtensions.NotEmptyOrWhiteSpace(subject, nameof(subject));
        Body = ValidationExtensions.NotEmptyOrWhiteSpace(input, "body");
        IsBodyHtml = true;
        MailResources = mailResources ?? new MailResources();
    }
}