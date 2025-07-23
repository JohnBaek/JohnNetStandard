using System.Collections.Concurrent;
using System.Reflection;
using JohnIsDev.Core.Features.Attributes;
using JohnIsDev.Core.Models.Common.Query;

namespace JohnIsDev.Core.Features.Helpers;

/// <summary>
/// Provides utility methods for extracting query search metadata from properties of a specified type that are annotated
/// with the <see cref="QueryMetaConvertAttribute"/>.
/// </summary>
public static class QuerySearchMapper
{
    /// <summary>
    /// A thread-safe, concurrent dictionary that serves as a cache for query search metadata.
    /// This field maps a type to a list of <see cref="RequestQuerySearchMeta"/> objects, which contain metadata extracted
    /// from properties of the specified type that are annotated with <see cref="QueryMetaConvertAttribute"/>.
    /// </summary>
    private static readonly ConcurrentDictionary<Type, List<RequestQuerySearchMeta>> _searchMetaCache = new();
    
    /// <summary>
    /// Extracts metadata related to query search from properties of a specified type that are annotated
    /// with the <see cref="QueryMetaConvertAttribute"/>.
    /// </summary>
    /// <typeparam name="T">The type whose properties are inspected for query search metadata attributes.</typeparam>
    /// <returns>A list of <see cref="RequestQuerySearchMeta"/> objects containing metadata derived from the annotated properties.</returns>
    public static List<RequestQuerySearchMeta> ExtractSearchMetaFromAttributes<T>()
    {
        return _searchMetaCache.GetOrAdd(typeof(T), _ =>
        {
            List<RequestQuerySearchMeta> searchMetas = [];
            List<PropertyInfo> properties = typeof(T).GetProperties().ToList();

            foreach (PropertyInfo property in properties)
            {
                // Get "QueryMetaAttribute" Attributes
                IEnumerable<QueryMetaConvertAttribute> attributes = property
                    .GetCustomAttributes(typeof(QueryMetaConvertAttribute), false)
                    .Cast<QueryMetaConvertAttribute>();

                foreach (QueryMetaConvertAttribute attribute in attributes)
                {
                    searchMetas.Add(new RequestQuerySearchMeta
                    {
                        Field = property.Name,
                        SearchType = attribute.SearchType
                    });
                }
            }

            return searchMetas;
        });
    }
}