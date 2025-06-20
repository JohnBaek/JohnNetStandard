
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace JohnIsDev.Core.Extensions;

/// <summary>
/// QueryableExtensions
/// </summary>
public static class QueryableExtensions
{
    /// <summary>
    /// FirstOrDefaultAsync To TSource -> TDestination
    /// </summary>
    /// <param name="query">query</param>
    /// <param name="predicate">predicate</param>
    /// <typeparam name="TSource">TSource</typeparam>
    /// <typeparam name="TDestination">TDestination</typeparam>
    /// <returns></returns>
    public static async Task<TDestination?> FirstOrDefaultAsync<TSource, TDestination>(
        this IQueryable<TSource> query,
        Expression<Func<TSource, bool>>? predicate = null)
        where TDestination : class
    {
        TSource? entity;

        // Get Entity
        if(predicate != null)
            entity = await query.FirstOrDefaultAsync(predicate);
        else
            entity = await query.FirstOrDefaultAsync();
        
        // If is entity null
        if (entity == null)
            return default;
        
        // Get Response Type
        return entity.FromCopyValue<TDestination>();
    }
    
    
    /// <summary>
    /// ToListAsync 
    /// </summary>
    /// <param name="query">query</param>
    /// <param name="predicate">predicate</param>
    /// <typeparam name="TSource">TSource</typeparam>
    /// <typeparam name="TDestination">TDestination</typeparam>
    /// <returns></returns>
    public static async Task<List<TDestination>> ToListAsync<TSource, TDestination>(
        this IQueryable<TSource> query,
        Expression<Func<TSource, bool>>? predicate = null)
        where TDestination : class, new()
    {
        List<TSource> sourceCollection;
        
        // Get Entity
        if(predicate != null)
            sourceCollection = await query.Where(predicate).ToListAsync();
        else
            sourceCollection = await query.ToListAsync();
        
        List<TDestination> result = new List<TDestination>();
        foreach (var source in sourceCollection)
        {
            if(source != null)
                result.Add(source.FromCopyValue<TDestination>());
        }
        return result;
    }
}