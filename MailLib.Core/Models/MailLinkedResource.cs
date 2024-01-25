namespace MailLib.Models;

public class MailLinkedResource
{
    public string ContentId { get; set; } = string.Empty;
    public string ContentPath { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public byte[] ContentBytes { get; set; } = Array.Empty<byte>();
}