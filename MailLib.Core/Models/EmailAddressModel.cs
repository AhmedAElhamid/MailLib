using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MailLib.Models;

public class EmailAddressModel
{
    public string Email { get; private set; }

    public string Name { get; private set; }

    [JsonConstructor]
    public EmailAddressModel(string email, string name)
    {
        if (string.IsNullOrWhiteSpace(email) || !new EmailAddressAttribute().IsValid(email))
            throw new ArgumentException("Email '" + email + "' is not a valid email.");
        Email = email;
        Name = name;
    }
}