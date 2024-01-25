using System.Text;
using MailLib.Models;

namespace MailLib.Extensions;

internal static class PlaceholderExtension
{
    public static string ReplacePlaceholders(
        this string htmlBody,
        List<TemplatePlaceholder>? placeholders)
    {
        if (placeholders.IsEmpty()) return htmlBody;

        var htmlBodyBuilder = placeholders!
            .Aggregate(new StringBuilder(htmlBody), (current, placeholder) =>
                current.Replace(placeholder.Placeholder.Trim(), placeholder.Value));

        return htmlBodyBuilder.ToString();
    }
}