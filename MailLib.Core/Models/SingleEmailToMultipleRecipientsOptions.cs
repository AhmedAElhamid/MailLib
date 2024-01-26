using System.Text.Json.Serialization;
using MailLib.Extensions;
using MailLib.Interfaces;

namespace MailLib.Models;

public class SingleEmailToMultipleRecipientsOptions : ISingleEmailToMultipleRecipientsOptions
{
    public List<EmailAddressModel> To { get; private set; }

    public string Subject { get; private set; }

    public string Body { get; private set; }

    public bool IsHtmlBody { get; private set; }

    public string? PlainTextContent => !IsHtmlBody ? Body : null;

    public MailResources MailResources { get; set; }
    public string? HtmlContent => !IsHtmlBody ? null : Body;

    public SingleEmailToMultipleRecipientsOptions(
        List<EmailAddressModel> to,
        string subject,
        string body,
        List<TemplatePlaceholder>? placeholders = default,
        bool isHtmlBody = true,
        MailResources? mailResources = default
    )
    {
        body = body.ReplacePlaceholders(placeholders);
        To = ValidationExtensions.NotEmptyCollection(to, nameof(to)).ToList();
        Subject = ValidationExtensions.NotEmptyOrWhiteSpace(subject, nameof(subject));
        Body = ValidationExtensions.NotEmptyOrWhiteSpace(body, nameof(body));
        IsHtmlBody = isHtmlBody;
        MailResources = mailResources ?? new MailResources();
    }
}