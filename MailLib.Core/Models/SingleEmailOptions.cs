using System.Text.Json.Serialization;
using MailLib.Extensions;
using MailLib.Interfaces;

namespace MailLib.Models;

public class SingleEmailOptions : ISingleEmailOptions
{
    public EmailAddressModel To { get; private set; }

    public string Subject { get; private set; }

    public string Body { get; private set; }

    public MailResources MailResources { get; set; }
    public bool IsHtmlBody { get; private set; }

    public string? PlainTextContent => !IsHtmlBody ? Body : null;

    public string? HtmlContent => !IsHtmlBody ? null : Body;


    [JsonConstructor]
    public SingleEmailOptions(
        EmailAddressModel to,
        string subject,
        string body,
        List<TemplatePlaceholder>? placeholders = default,
        bool isHtmlBody = true,
        MailResources? mailResources = default
    )
    {
        body = body.ReplacePlaceholders(placeholders);
        To = to;
        Subject = ValidationExtensions.NotEmptyOrWhiteSpace(subject, nameof(subject));
        Body = ValidationExtensions.NotEmptyOrWhiteSpace(body, "body");
        IsHtmlBody = isHtmlBody;
        MailResources = mailResources ?? new MailResources();
    }
}