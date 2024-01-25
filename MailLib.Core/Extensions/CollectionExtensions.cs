namespace MailLib.Extensions;

public static class CollectionExtensions
{
    public static IEnumerable<T> AsNotNull<T>(this IEnumerable<T>? collection)
        => collection ?? Enumerable.Empty<T>();

    public static bool IsEmpty<T>(this IEnumerable<T>? collection)
        => !collection.AsNotNull().Any();
}