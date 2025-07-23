using JohnIsDev.Core.Models.Common.Query;
using JohnIsDev.Core.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace JohnIsDev.Core.EntityFramework.EFQueryProvider.Interfaces;

/// <summary>
/// QueryBuilder 
/// </summary>
public interface IQueryBuilder<TDbContext> where TDbContext : DbContext
{
    /// <summary>
    /// Build a Query
    /// </summary>
    /// <param name="requestQuery">requestQuery</param>
    /// <typeparam name="T"></typeparam>
    /// <returns>IQueryable</returns>
    IQueryable<T>? BuildQuery<T>(RequestQuery requestQuery) where T : class;
    
    /// <summary>
    /// Build a Query
    /// </summary>
    /// <param name="requestQuery">requestQuery</param>
    /// <param name="queryable">queryable</param>
    /// <typeparam name="T"></typeparam>
    /// <returns>IQueryable</returns>
    IQueryable<T>? BuildQuery<T>(RequestQuery requestQuery, IQueryable<T> queryable) where T : class;
    
    /// <summary>
    /// Converts to a QuerySearch from RequestQuery
    /// </summary>
    /// <param name="requestQuery"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns>QuerySearch</returns>
    IEnumerable<QuerySearch> ConvertToQuerySearchList<T>(RequestQuery requestQuery) where T : class;
    //
    // /// <summary>
    // /// Converts the given queryable and request query into a response list.
    // /// </summary>
    // /// <param name="queryable">The queryable containing data to be converted.</param>
    // /// <param name="requestQuery">The request query specifying pagination and query parameters.</param>
    // /// <typeparam name="T">The type of the data contained in the queryable and response list.</typeparam>
    // /// <returns>A ResponseList containing the paginated data and related metadata, or null if conversion fails.</returns>
    // Task<ResponseList<T>> ToResponseListAsync<T>(IQueryable<T> queryable, RequestQuery requestQuery) where T : class;
    //
    //
    // /// <summary>
    // /// Converts a queryable collection to a paginated response list with automatic mapping from the source type to the target type.
    // /// </summary>
    // /// <typeparam name="TQueryable">The type of the source elements in the queryable collection.</typeparam>
    // /// <typeparam name="TConvert">The type to which the source elements are converted.</typeparam>
    // /// <param name="queryable">The queryable collection to be converted into a response list.</param>
    // /// <param name="requestQuery">The request query containing pagination and filtering parameters.</param>
    // /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="ResponseList{TConvert}"/> containing the paginated and mapped results.</returns>
    // Task<ResponseList<TConvert>> ToResponseListAutoMappingAsync<TQueryable, TConvert>(IQueryable<TQueryable> queryable,
    //     RequestQuery requestQuery)
    //     where TConvert : class
    //     where TQueryable : class;
}