using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MailLib.Extensions;
using MailLib.Interfaces;
using MailLib.Models;
using Microsoft.Extensions.Options;
using MimeKit;

namespace MailLib.SMTP;

public class SmtpEmailService(
    IOptions<SmtpConfiguration> smtpConfiguration,
    CancellationToken cancellationToken = default)
    : IEmailService
{
    private readonly SmtpConfiguration _smtpConfiguration = smtpConfiguration.Value;

    public async Task SendEmail(SingleEmailOptions options)
    {
        if (string.IsNullOrEmpty(options.To.Email)) return;

        var email = await GetEmail(_smtpConfiguration.From,
            options.To.Email, options.Subject, options.Body, options.MailResources, cancellationToken);

        await Send(_smtpConfiguration, email, cancellationToken);
    }

    public async Task SendEmail(SingleEmailToMultipleRecipientsOptions options)
    {
        if (options.To.IsEmpty()) return;
        var recipients = options.To.AsNotNull().Select(to => to.Email).ToList();

        var email = await GetEmail(_smtpConfiguration.From,
            recipients, options.Subject, options.Body, options.MailResources, cancellationToken);

        await Send(_smtpConfiguration, email, cancellationToken);
    }

    public async Task SendEmail(UserSpecificEmailBodyToMultipleRecipientsOptions options)
    {
        if (options.EmailOptionsMap.IsEmpty()) return;

        var emails = await GetEmails(_smtpConfiguration.From, options.Subject,
            GetUsersEmails(options.EmailOptionsMap),
            options.MailResources, cancellationToken);

        await BulkSend(_smtpConfiguration, emails, cancellationToken);
    }

    private static List<UserEmail> GetUsersEmails(IEnumerable<SingleEmailOptions> optionsCollection)
        => optionsCollection.Select(options => new UserEmail
        {
            To = options.To.Email,
            Body = options.Body
        }).ToList();

    private static async Task Send(SmtpConfiguration configuration,
        MimeMessage email, CancellationToken cancellationToken = default)
    {
        using var client = new SmtpClient();
        await ConnectAndAuthenticate(client, configuration, cancellationToken);
        await client.SendAsync(email, cancellationToken);
        await Disconnect(client, cancellationToken);
    }

    private static async Task BulkSend(SmtpConfiguration configuration,
        List<MimeMessage> emails, CancellationToken cancellationToken = default)
    {
        using var client = new SmtpClient();
        await ConnectAndAuthenticate(client, configuration, cancellationToken);
        foreach (var email in emails) await client.SendAsync(email, cancellationToken);
        await Disconnect(client, cancellationToken);
    }

    private static async Task<List<MimeMessage>> GetEmails(string from, string subject, List<UserEmail> userEmails,
        MailResources mailResources, CancellationToken cancellationToken = default)
    {
        var emails = new List<MimeMessage>();
        var bodyBuilder = await AddAttachmentsAndResources(new BodyBuilder(),
            mailResources, cancellationToken);
        var fromAddress = MailboxAddress.Parse(from);
        foreach (var userEmail in userEmails)
        {
            var email = new MimeMessage();
            email.From.Add(fromAddress);
            email.Subject = subject;
            email.To.Add(MailboxAddress.Parse(userEmail.To));
            bodyBuilder.HtmlBody = userEmail.Body;
            email.Body = bodyBuilder.ToMessageBody();
            emails.Add(email);
        }

        return emails;
    }

    private static async Task<MimeMessage> GetEmail(string from, string to, string subject, string body,
        MailResources mailResources, CancellationToken cancellationToken = default)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(from));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;
        email.Body = await GetMailBody(body, mailResources, cancellationToken);
        return email;
    }

    private static async Task<MimeMessage> GetEmail(string from, List<string> tos, string subject, string body,
        MailResources mailResources, CancellationToken cancellationToken = default)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(from));
        tos.ForEach(to => email.To.Add(MailboxAddress.Parse(to)));
        email.Subject = subject;
        email.Body = await GetMailBody(body, mailResources, cancellationToken);
        return email;
    }

    private static async Task<MimeEntity> GetMailBody(string body, MailResources mailResources,
        CancellationToken cancellationToken = default)
    {
        var builder = new BodyBuilder { HtmlBody = body };
        await AddAttachmentsAndResources(builder, mailResources, cancellationToken);
        return builder.ToMessageBody();
    }

    private static async Task<BodyBuilder> AddAttachmentsAndResources(BodyBuilder builder, MailResources mailResources,
        CancellationToken cancellationToken = default)
    {
        foreach (var r in mailResources.LinkedResources.AsNotNull()!)
        {
            var resource = !string.IsNullOrEmpty(r.ContentPath)
                ? await builder.LinkedResources.AddAsync(r.ContentPath, cancellationToken)
                : builder.LinkedResources.Add(r.ContentId,
                    r.ContentBytes,
                    ContentType.Parse(r.ContentType));
            resource.ContentId = r.ContentId;
        }

        foreach (var a in mailResources.Attachments.AsNotNull()!)
        {
            if (!string.IsNullOrEmpty(a.ContentPath))
                await builder.Attachments.AddAsync(a.ContentPath, cancellationToken);
            else
                builder.Attachments.Add(a.ContentId, a.ContentBytes, ContentType.Parse(a.ContentType));
        }

        return builder;
    }

    private static async Task ConnectAndAuthenticate(IMailService client, SmtpConfiguration configuration,
        CancellationToken cancellationToken)
    {
        client.ServerCertificateValidationCallback = (_, _, _, _) => true;
        client.AuthenticationMechanisms.Remove("XOAUTH2");
        await client.ConnectAsync(configuration.Host, configuration.Port,
            SecureSocketOptions.StartTlsWhenAvailable, cancellationToken);
        if (!string.IsNullOrEmpty(configuration.User) && !string.IsNullOrEmpty(configuration.Password))
            await client.AuthenticateAsync(configuration.User, configuration.Password, cancellationToken);
    }

    private static async Task Disconnect(IMailService client, CancellationToken cancellationToken)
    {
        await client.DisconnectAsync(true, cancellationToken);
    }
}

internal class UserEmail
{
    public string To { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}