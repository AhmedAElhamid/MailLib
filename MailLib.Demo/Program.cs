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

var user = new EmailAddressModel("hirom41547@grassdev.com", "hirom41547");

var singleRecipientOptions = new SingleEmailOptions(
    user,
    subject,
    htmlBody,
    placeholders,
    mailResources);

await emailService.SendEmail(singleRecipientOptions);

var users = new List<EmailAddressModel>
{
    new("hirom41547@grassdev.com", "hirom41547"),
    new("teltuzakki@gufum.com", "teltuzakki"),
};

var multipleRecipientsOptions = new SingleEmailToMultipleRecipientsOptions(
    users,
    subject,
    htmlBody,
    placeholders,
    mailResources
);

await emailService.SendEmail(multipleRecipientsOptions);
return;

static string GetTemplatePath(string templateName)
    => Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "EmailTemplates", templateName);

static string GetAttachmentPath(
    string attachmentName) =>
    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "EmailTemplates", "Attachments", attachmentName);