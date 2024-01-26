namespace MailLib.Extensions;

public static class HtmlTemplateHelper
{
    public static async Task<string> ExtractBodyFromHtmlTemplate(string templatePath)
    {
        if (string.IsNullOrEmpty(templatePath)) throw new ArgumentNullException(nameof(templatePath));
        using var streamReader = File.OpenText(templatePath);
        return await streamReader.ReadToEndAsync();
    }
}