using MimeKit;

namespace MailLib.SMTP.Extensions;

public static class HtmlTemplateHelper
{
    public static string ExtractStringFromHtml(string templatePath)
    {
        if (string.IsNullOrEmpty(templatePath)) throw new ArgumentNullException(nameof(templatePath));
        var bodyBuilder = new BodyBuilder();
        using (var streamReader = File.OpenText(templatePath))
            bodyBuilder.HtmlBody = streamReader.ReadToEnd();
        return bodyBuilder.HtmlBody;
    }
}