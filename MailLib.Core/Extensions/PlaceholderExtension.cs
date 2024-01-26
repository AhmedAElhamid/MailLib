using System.Text;
using MailLib.Models;

namespace MailLib.Extensions;

internal static class PlaceholderExtension
{
    public static string ReplacePlaceholders(
        this string body,
        List<TemplatePlaceholder>? placeholders)
    {
        if (placeholders.IsEmpty()) return body;

        var bodyBuilder = placeholders!
            .Aggregate(new StringBuilder(body), (current, placeholder) =>
                current.Replace(placeholder.Placeholder.Trim(), placeholder.Value));

        return bodyBuilder.ToString();
    }
}