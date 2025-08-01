using JohnIsDev.Core.EntityFramework.EFQueryProvider.Interfaces;
using JohnIsDev.Core.Extensions;
using JohnIsDev.Core.Models.Common.Enums;
using JohnIsDev.Core.Models.Common.Query;
using JohnIsDev.Core.Models.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace JohnIsDev.Core.EntityFramework.EFQueryProvider.Implements;

/// <summary>
/// Provides an interface to execute operations within a transactional context.
/// Ensures that the specified operation is executed with transactional safety mechanisms.
/// </summary>
public class QueryExecutor<TDbContext>(
      ILogger<QueryExecutor<TDbContext>> logger
    ,TDbContext dbContext) : IQueryExecutor<TDbContext>
    where TDbContext : DbContext
{
    /// <summary>
    /// Executes a specified operation within a transactional context.
    /// Ensures transactional safety by committing if the operation succeeds,
    /// or rolling back if an error occurs during execution.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response that the operation generates, which must inherit from Response and have a parameterless constructor.</typeparam>
    /// <param name="operation">A function that contains the operation to be executed within a transaction. The function takes an instance of <typeparamref name="TDbContext"/> as input and produces a <typeparamref name="TResponse"/> as output.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the response of the operation. If the operation fails, the response contains error details.</returns>
    public async Task<TResponse> ExecuteWithTransactionAutoCommitAsync<TResponse>(Func<TDbContext, Task<TResponse>> operation)
        where TResponse : Response, new()
    {
        // Begin transactions
        await using IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync();
        try
        {
            TResponse result = await operation(dbContext);
            await transaction.CommitAsync();
            return result;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            logger.LogError(e, e.Message);
            return new TResponse
            {
                Result = EnumResponseResult.Error,
                Code = "TRANSACTION_ERROR",
                Message = "Error has been occurred. while executing transaction."
            };
        }
    }

    /// <summary>
    /// Executes the specified query asynchronously and returns a paginated response containing the results.
    /// </summary>
    /// <param name="queryable">The queryable object representing the database query to execute.</param>
    /// <param name="requestQuery">An object containing pagination parameters such as skip and page count.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="ResponseList{TDbContext}"/> with the query results and pagination details.</returns>
    public async Task<ResponseList<TEntity>> ExecuteAutoPaginateAsync<TEntity>(
        IQueryable<TEntity> queryable,
        RequestQuery requestQuery) where TEntity : class
    {
        try
        {
            // Select a total count
            int totalCount = await queryable.AsNoTracking().CountAsync();
            
            // Select a paged list
            List<TEntity> items = await queryable.AsNoTracking()
                .Skip(requestQuery.Skip)
                .Take(requestQuery.PageCount)
                .ToListAsync();
            
            return new ResponseList<TEntity>(EnumResponseResult.Success, "", "", items)
            {
                TotalCount = totalCount ,
                Skip = requestQuery.Skip ,
                PageCount = requestQuery.PageCount 
            };
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return new ResponseList<TEntity>(EnumResponseResult.Error, "COMMON_DATABASE_ERROR","", []);
        }
    }


    /// <summary>
    /// Converts a queryable source into a paginated response list based on the specified request query.
    /// </summary>
    /// <typeparam name="T">The type of elements in the queryable source.</typeparam>
    /// <param name="queryable">The queryable source to apply the request query filters and pagination.</param>
    /// <param name="requestQuery">The request query containing pagination and filtering information.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="ResponseList{T}"/>
    /// populated with paginated and filtered data, along with additional pagination metadata.</returns>
    public async Task<ResponseList<T>> ToResponseListAsync<T>(IQueryable<T> queryable, RequestQuery requestQuery)
        where T : class
    {
        try
        {
            // Select a total count
            int totalCount = await queryable.AsNoTracking().CountAsync();
            
            // Select a paged list
            List<T> items = await queryable.AsNoTracking()
                .Skip(requestQuery.Skip)
                .Take(requestQuery.PageCount)
                .ToListAsync();
            
            return new ResponseList<T>(EnumResponseResult.Success, "", "", items)
            {
                TotalCount = totalCount ,
                Skip = requestQuery.Skip ,
                PageCount = requestQuery.PageCount 
            };
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return new ResponseList<T>(EnumResponseResult.Error, "COMMON_DATABASE_ERROR","", []);
        }
    }
    
    
    /// <summary>
    /// Converts a queryable data set to a response list with automatic mapping from the source type to the target type.
    /// </summary>
    /// <typeparam name="TQueryable">The type of the source queryable data.</typeparam>
    /// <typeparam name="TConvert">The type to which the source data will be mapped.</typeparam>
    /// <param name="queryable">The source queryable data to be converted.</param>
    /// <param name="requestQuery">The request query object containing filters, sorting, and pagination criteria.</param>
    /// <returns>A response list of type TConvert containing the converted data or an error status if the operation fails.</returns>
    public async Task<ResponseList<TConvert>> ToResponseListAutoMappingAsync<TQueryable, TConvert>(
        IQueryable<TQueryable> queryable, RequestQuery requestQuery)
        where TConvert : class
        where TQueryable : class
    {
        try
        {
            // Get a data ResponseList<TQueryable>
            ResponseList<TQueryable> result = await ToResponseListAsync(queryable, requestQuery);
            
            // Convert TQueryable to TConvert List Collection
            List<TConvert> convertList = new List<TConvert>(result.Items.Select(resultItem => resultItem.FromCopyValue<TConvert>()));
            
            // Convert ResponseList<TConvert>
            return new ResponseList<TConvert>
            {
                Result = result.Result,
                Code = result.Code,
                Message = result.Message,
                TotalCount = result.TotalCount,
                Skip = result.Skip,
                PageCount = result.PageCount,
                Items = convertList
            };
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return new ResponseList<TConvert>(EnumResponseResult.Error, "COMMON_DATABASE_ERROR","", []);
        }
    }
}