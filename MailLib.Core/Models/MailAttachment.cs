namespace MailLib.Models;

public class MailAttachment
{
    public string ContentId { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public byte[] ContentBytes { get; set; } = Array.Empty<byte>();
    public string ContentPath { get; set; } = string.Empty;
}