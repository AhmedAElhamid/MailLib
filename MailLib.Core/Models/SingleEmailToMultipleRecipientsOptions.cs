using System.Text.Json.Serialization;
using MailLib.Extensions;
using MailLib.Interfaces;

namespace MailLib.Models;

public class SingleEmailToMultipleRecipientsOptions : ISingleEmailToMultipleRecipientsOptions
{
    public List<EmailAddressModel> To { get; private set; } = new();

    public string Subject { get; private set; } = string.Empty;

    public string Body { get; private set; } = string.Empty;

    public bool IsBodyHtml { get; private set; }

    public string? PlainTextContent => !IsBodyHtml ? Body : null;

    public MailResources MailResources { get; set; } = new();
    public string? HtmlContent => !IsBodyHtml ? null : Body;

    [JsonConstructor]
    private SingleEmailToMultipleRecipientsOptions()
    {
    }

    public SingleEmailToMultipleRecipientsOptions(
        List<EmailAddressModel> to,
        string subject,
        string body,
        MailResources? mailResources = default,
        bool isBodyHtml = false
    )
    {
        To = ValidationExtensions.NotEmptyCollection(to, nameof(to)).ToList();
        Subject = ValidationExtensions.NotEmptyOrWhiteSpace(subject, nameof(subject));
        Body = ValidationExtensions.NotEmptyOrWhiteSpace(body, nameof(body));
        IsBodyHtml = isBodyHtml;
        MailResources = mailResources ?? new MailResources();
    }

    public SingleEmailToMultipleRecipientsOptions(
        List<EmailAddressModel> to,
        string subject,
        string htmlBody,
        List<TemplatePlaceholder> placeholders,
        MailResources? mailResources = default)
    {
        var input = htmlBody.ReplacePlaceholders(placeholders);
        To = to;
        Subject = ValidationExtensions.NotEmptyOrWhiteSpace(subject, nameof(subject));
        Body = ValidationExtensions.NotEmptyOrWhiteSpace(input, "body");
        IsBodyHtml = true;
        MailResources = mailResources ?? new MailResources();
    }
}