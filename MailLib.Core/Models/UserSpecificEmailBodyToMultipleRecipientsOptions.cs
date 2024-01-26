using MailLib.Extensions;

namespace MailLib.Models;

public class UserSpecificEmailBodyToMultipleRecipientsOptions
{
    public ICollection<SingleEmailOptions> EmailOptionsMap { get; private set; } = new List<SingleEmailOptions>();

    public string Subject { get; private set; }

    public MailResources MailResources { get; set; }


    public UserSpecificEmailBodyToMultipleRecipientsOptions(
        IEnumerable<EmailAddressModel> to,
        string subject,
        string htmlBody,
        Func<EmailAddressModel, List<TemplatePlaceholder>> placeholders,
        MailResources? mailResources = default
    )
    {
        Subject = ValidationExtensions.NotEmptyOrWhiteSpace(subject, nameof(subject));
        foreach (var emailOptions in from recipient in to
                 let input = htmlBody.ReplacePlaceholders(placeholders(recipient))
                 select new SingleEmailOptions(recipient, subject, input, mailResources))
            EmailOptionsMap.Add(emailOptions);
        MailResources = mailResources ?? new MailResources();
    }
}