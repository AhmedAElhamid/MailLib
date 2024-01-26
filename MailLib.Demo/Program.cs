// See https://aka.ms/new-console-template for more information

using MailLib.Models;
using MailLib.SMTP;
using MailLib.SMTP.Extensions;
using Microsoft.Extensions.Options;


var options = new SmtpConfiguration("smtp.gmail.com", 587,
    "user@gmail.com", "user@gmail.com", "app-password");

var smtpConfiguration = Options.Create(options);
var emailService = new SmtpEmailService(smtpConfiguration);

const string subject = "Welcome To Application";


const string templateName = "invite-user.html";
var htmlBody = HtmlTemplateHelper.ExtractStringFromHtml(GetTemplatePath(templateName));

List<TemplatePlaceholder> placeholders =
[
    new TemplatePlaceholder { Placeholder = "{user-name}", Value = "User" }
];

var mailResources = new MailResources
{
    LinkedResources = new List<MailLinkedResource>
    {
        new()
        {
            ContentPath = GetAttachmentPath("app-logo.png"),
            ContentId = "app-logo.png"
        },
        new()
        {
            ContentPath = GetAttachmentPath("arrow.png"),
            ContentId = "arrow.png"
        }
    }
};

// This is an example of sending a single email to a single recipient

var user = new EmailAddressModel("yuknigukko@gufum.com", "yuknigukko");

var singleRecipientOptions = new SingleEmailOptions(
    user,
    subject,
    htmlBody,
    placeholders,
    mailResources);

await emailService.SendEmail(singleRecipientOptions);

// This is an example of sending a single email to multiple recipients

var users = new List<EmailAddressModel>
{
    new("yuknigukko@gufum.com", "yuknigukko"),
    new("hirom41547@grassdev.com", "hirom41547"),
};

var multipleRecipientsOptions = new SingleEmailToMultipleRecipientsOptions(
    users,
    subject,
    htmlBody,
    placeholders,
    mailResources
);

await emailService.SendEmail(multipleRecipientsOptions);

// This is a more advanced example of sending emails to multiple recipients

var userSpecificEmailBodyToMultipleRecipientsOptions = new UserSpecificEmailBodyToMultipleRecipientsOptions(
    users,
    subject,
    htmlBody,
    recipient => [new TemplatePlaceholder { Placeholder = "{user-name}", Value = recipient.Name }],
    mailResources
);

await emailService.SendEmail(userSpecificEmailBodyToMultipleRecipientsOptions);

return;

static string GetTemplatePath(string templateName)
    => Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "EmailTemplates", templateName);

static string GetAttachmentPath(
    string attachmentName) =>
    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "EmailTemplates", "Attachments", attachmentName);