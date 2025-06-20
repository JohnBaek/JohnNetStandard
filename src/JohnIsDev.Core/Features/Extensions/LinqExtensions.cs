namespace JohnIsDev.Core.Features.Extensions;

/// <summary>
/// Extensions of LINQ
/// </summary>
public static class LinqExtensions
{
    /// <summary>
    /// ToJoin With String
    /// </summary>
    /// <param name="source"></param>
    /// <param name="separator"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static string WithJoinString<T>(this IEnumerable<T> source, string separator)
    {
        // Invalid
        if(source == null) 
            throw new ArgumentNullException(nameof(source));
        
        return string.Join(separator, source);
    }
}