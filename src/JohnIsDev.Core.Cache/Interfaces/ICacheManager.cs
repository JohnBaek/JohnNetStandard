namespace JohnIsDev.Core.Cache.Interfaces;

/// <summary>
/// ICacheService
/// </summary>
public interface ICacheManager
{
    /// <summary>
    /// Get Cached Data
    /// </summary>
    /// <param name="key">Key</param>
    /// <typeparam name="T">Type</typeparam>
    /// <returns>T</returns>
    T? Get<T>(string key);
    
    /// <summary>
    /// Get Cached Data
    /// </summary>
    /// <param name="key">Key</param>
    /// <typeparam name="T">Type</typeparam>
    /// <returns>T</returns>
    Task<T?> GetAsync<T>(string key) where T : class;
    
    /// <summary>
    /// Set Cached Data
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="value">T value</param>
    /// <param name="expirationTime">expiredTime</param>
    void Set<T>(string key, T value, TimeSpan? expirationTime = null);
    
    /// <summary>
    /// Set Cached Data
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="value">T value</param>
    /// <param name="expirationTime">expiredTime</param>
    Task SetAsync<T>(string key, T value, TimeSpan? expirationTime = null);
    
    /// <summary>
    /// Remove Cached Data
    /// </summary>
    /// <param name="key">key</param>
    void Remove(string key);
    
    /// <summary>
    /// Remove Cached Data
    /// </summary>
    /// <param name="key">key</param>
    Task RemoveAsync(string key);

    /// <summary>
    /// Contains Key?
    /// </summary>
    /// <param name="key">key</param>
    public bool ContainsKey(string key);

    /// <summary>
    /// GetOrSetCache Async
    /// </summary>
    /// <param name="cacheKey"></param>
    /// <param name="cacheDuration"></param>
    /// <param name="callback"></param>
    public Task<List<T>> GetOrSetCacheAsync<T>(
        string cacheKey,
        TimeSpan cacheDuration,
        Func<Task<List<T>>> callback);

    /// <summary>
    /// Executes the provided asynchronous operation and evicts the cached data associated with the specified key.
    /// </summary>
    /// <param name="cacheKey">The key of the cache entry to be evicted.</param>
    /// <param name="callback">The asynchronous operation to execute.</param>
    /// <typeparam name="T">The type of the data to be returned.</typeparam>
    /// <returns>The result of the asynchronous operation of type T.</returns>
    Task<T> ExecuteAndEvictCacheAsync<T>(string cacheKey, Func<Task<T>> callback);
}