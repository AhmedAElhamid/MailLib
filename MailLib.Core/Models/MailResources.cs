namespace MailLib.Models;

public class MailResources
{
    public ICollection<MailLinkedResource> LinkedResources { get; set; } = new List<MailLinkedResource>();
    public ICollection<MailAttachment> Attachments { get; set; } = new List<MailAttachment>();
}