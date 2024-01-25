namespace MailLib.Extensions;

public class ValidationExtensions
{
    public static string NotEmptyOrWhiteSpace(string value, string name)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"'{name}' cannot be null or whitespace.", name);
        return value;
    }

    public static ICollection<T> NotEmptyCollection<T>(ICollection<T> collection, string name)
    {
        if (collection == null || collection.Count == 0)
            throw new ArgumentException($"'{name}' cannot be null or empty.", name);
        return collection;
    }
}