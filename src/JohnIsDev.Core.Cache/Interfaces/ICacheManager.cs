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
}