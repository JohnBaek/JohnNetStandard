using System.Text.Json;
using JohnIsDev.Core.Cache.Interfaces;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

#pragma warning disable CS8602 // Dereference of a possibly null reference.

namespace JohnIsDev.Core.Cache.Implements;

/// <summary>
/// A Redis Cache Memory Service Provider
/// </summary>
public class CacheManagerForRedisProvider(ILogger<CacheManagerForRedisProvider> logger, IDatabase redis, IOutputCacheStore cacheStore) : ICacheManager
{
    /// <summary>
    /// a Locking  
    /// </summary>
    private readonly AsyncKeyedLock<string> _asyncLock = new ();
    
    /// <summary>
    /// Get Cached Data
    /// </summary>
    /// <param name="key">Key</param>
    /// <typeparam name="T">Type</typeparam>
    /// <returns>T</returns>
    public T? Get<T>(string key)
    {
        try
        {
            // Gets string data from redis cache by Key
            string? jsonCharacters = redis.StringGet(key);
            return string.IsNullOrEmpty(jsonCharacters) ? default : JsonSerializer.Deserialize<T>(jsonCharacters);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
        }
        return default;
    }

    public async Task<T?> GetAsync<T>(string key) where T : class
    {
        try
        {
            // Gets string data from redis cache by Key
            string? jsonCharacters = await redis.StringGetAsync(key);
            return string.IsNullOrEmpty(jsonCharacters) ? null : JsonSerializer.Deserialize<T>(jsonCharacters);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
        }
        return null;
    }

    /// <summary>
    /// Set Cached Data
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="value">T value</param>
    /// <param name="expirationTime">expiredTime</param>
    public void Set<T>(string key, T value, TimeSpan? expirationTime = null)
    {
        try
        {
            string jsonCharacters = JsonSerializer.Serialize(value);

            // If it requires sets expire time
            if (expirationTime != null)
            {
                redis.StringSet(key, jsonCharacters, expirationTime.Value);
            }
            // If not
            else
            {
                redis.StringSet(key, jsonCharacters);
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expirationTime = null)
    {
        try
        {
            string jsonCharacters = JsonSerializer.Serialize(value);

            // If it requires sets expire time
            if (expirationTime != null)
            {
                await redis.StringSetAsync(key, jsonCharacters, expirationTime.Value);
            }
            // If not
            else
            {
                await redis.StringSetAsync(key, jsonCharacters);
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
        }
    }

    /// <summary>
    /// Remove Cached Data
    /// </summary>
    /// <param name="key">key</param>
    public void Remove(string key)
    {
        redis.KeyDelete(key);
    }

    public async Task RemoveAsync(string key)
    {
        await redis.KeyDeleteAsync(key);
    }

    /// <summary>
    /// Contains Key?
    /// </summary>
    /// <param name="key">key</param>
    /// <returns></returns>
    public bool ContainsKey(string key)
    {
        return redis.KeyExists(key);
    }

    /// <summary>
    /// GetOrSetCache Async
    /// </summary>
    /// <param name="cacheKey"></param>
    /// <param name="cacheDuration"></param>
    /// <param name="callback"></param>
    public async Task<List<T>> GetOrSetCacheAsync<T>(string cacheKey, TimeSpan cacheDuration, Func<Task<List<T>>> callback)
    {
        // Get data from Cache
        if (ContainsKey(cacheKey))
        {
            return await GetAsync<List<T>>(cacheKey) ?? new List<T>();
        }
        
        // Locking entry point for Prevent a Race problem 
        using (await _asyncLock.LockAsync(cacheKey))
        {
            // 데이터 로드
            var data = await callback();
        
            // 데이터 캐시 설정 (빈 리스트도 캐시)
            await SetAsync(cacheKey, data, cacheDuration);
        
            return data;
        }
    }

    /// <summary>
    /// Executes a specified callback function and evicts the associated cache entry.
    /// </summary>
    /// <param name="cacheKey">The key of the cache entry to be evicted.</param>
    /// <param name="callback">A function to be executed which retrieves the data.</param>
    /// <typeparam name="T">The type of data being retrieved.</typeparam>
    /// <returns>A task that represents the asynchronous operation. The task result contains the returned data of type T.</returns>
    public async Task<T> ExecuteAndEvictCacheAsync<T>(string cacheKey, Func<Task<T>> callback)
    {
        // Operation Data 
        var response = await callback();
            
        // Evict Cache
        await cacheStore.EvictByTagAsync(cacheKey, CancellationToken.None);
        return response;
    }
}
