using System.Text.Json.Serialization;
using MailLib.Extensions;

namespace MailLib.Models;

public class UserSpecificEmailBodyToMultipleRecipientsOptions
{
    public ICollection<SingleEmailOptions> EmailOptionsMap { get; private set; } = new List<SingleEmailOptions>();

    public string Subject { get; private set; }

    public MailResources MailResources { get; set; }

    public bool IsHtmlBody { get; private set; }


    [JsonConstructor]
    public UserSpecificEmailBodyToMultipleRecipientsOptions(
        IEnumerable<EmailAddressModel> to,
        string subject,
        string body,
        Func<EmailAddressModel, List<TemplatePlaceholder>> placeholders,
        bool isHtmlBody = true,
        MailResources? mailResources = default
    )
    {
        Subject = ValidationExtensions.NotEmptyOrWhiteSpace(subject, nameof(subject));
        foreach (var emailOptions in from recipient in to
                 select new SingleEmailOptions(recipient, subject, body,
                     placeholders(recipient), isHtmlBody, mailResources))
            EmailOptionsMap.Add(emailOptions);
        IsHtmlBody = isHtmlBody;
        MailResources = mailResources ?? new MailResources();
    }
}