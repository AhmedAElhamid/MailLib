// See https://aka.ms/new-console-template for more information

using MailLib.Models;
using MailLib.SMTP;
using MailLib.SMTP.Extensions;
using Microsoft.Extensions.Options;


var options = new SmtpConfiguration("smtp.gmail.com", 587,
    "user@gmail.com", "user@gmail.com", "app-password");

var users = new List<EmailAddressModel>
{
    new("yuknigukko@gufum.com", "yuknigukko"),
    new("hirom41547@grassdev.com", "hirom41547"),
};
var user = users.First();

var smtpConfiguration = Options.Create(options);
var emailService = new SmtpEmailService(smtpConfiguration);

const string subject = "Welcome To Application";
const string templateName = "invite-user.html";
var htmlBody = HtmlTemplateHelper.ExtractStringFromHtml(GetTemplatePath(templateName));
var mailResources = GetMailResources();


// This is an example of sending a single email to a single recipient with a text body

#region SingleEmailOptions

var singleRecipientOptionsWithTextBody = new SingleEmailOptions(
    user,
    subject,
    "Welcome to Application {user-name}",
    [new TemplatePlaceholder { Placeholder = "{user-name}", Value = user.Name }],
    false,
    mailResources: mailResources);

await emailService.SendEmail(singleRecipientOptionsWithTextBody);

#endregion

// This is an example of sending a single email to a single recipient with an html body

#region SingleEmailOptions

var singleRecipientOptionsWithHtmlBody = new SingleEmailOptions(
    user,
    subject,
    htmlBody,
    [new TemplatePlaceholder { Placeholder = "{user-name}", Value = user.Name }],
    mailResources: mailResources);

await emailService.SendEmail(singleRecipientOptionsWithHtmlBody);

#endregion

// This is an example of sending a single email to multiple recipients

#region SingleEmailToMultipleRecipientsOptions

var multipleRecipientsOptions = new SingleEmailToMultipleRecipientsOptions(
    users,
    subject,
    htmlBody,
    [new TemplatePlaceholder { Placeholder = "{user-name}", Value = "User" }],
    mailResources: mailResources
);

await emailService.SendEmail(multipleRecipientsOptions);

#endregion

// This is a more advanced example of sending emails to multiple recipients with user specific placeholders

#region UserSpecificEmailBodyToMultipleRecipientsOptions

var userSpecificEmailBodyToMultipleRecipientsOptions = new UserSpecificEmailBodyToMultipleRecipientsOptions(
    users,
    subject,
    htmlBody,
    recipient => [new TemplatePlaceholder { Placeholder = "{user-name}", Value = recipient.Name }],
    mailResources: mailResources
);

await emailService.SendEmail(userSpecificEmailBodyToMultipleRecipientsOptions);

#endregion

return;

static string GetTemplatePath(string templateName)
    => Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "EmailTemplates", templateName);

static string GetAttachmentPath(
    string attachmentName) =>
    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "EmailTemplates", "Attachments", attachmentName);


static MailResources GetMailResources()
    => new()
    {
        LinkedResources =
        [
            new MailLinkedResource
            {
                ContentPath = GetAttachmentPath("app-logo.png"),
                ContentId = "app-logo.png"
            },
            new MailLinkedResource
            {
                ContentPath = GetAttachmentPath("arrow.png"),
                ContentId = "arrow.png"
            }
        ]
    };