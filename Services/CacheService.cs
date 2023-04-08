using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace UserApi.Services;

public class CacheService : ICacheService
{
    private readonly IDistributedCache _distributedCache;

    public CacheService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        where T : class
    {
        var cachedValue = await _distributedCache.GetStringAsync(key, cancellationToken);

        if (cachedValue is null)
        {
            return null;
        }

        var value = JsonSerializer.Deserialize<T>(cachedValue);

        return value;
    }

    public async Task<T?> GetOrCreateAsync<T>(string key, Func<Task<T>> factory,
        DistributedCacheEntryOptions? options = default, CancellationToken cancellationToken = default) where T : class
    {
        var value = await GetAsync<T>(key, cancellationToken);
        if (value is not null) return value;
        value = await factory();
        await SetAsync(key, value, options, cancellationToken);
        return value;
    }

    public async Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions? options = default,
        CancellationToken cancellationToken = default)
        where T : class
    {
        var cacheValue = JsonSerializer.Serialize(value);
        if (options is null)
        {
            await _distributedCache.SetStringAsync(key, cacheValue, cancellationToken);
            return;
        }
        await _distributedCache.SetStringAsync(key, cacheValue, options, cancellationToken);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await _distributedCache.RemoveAsync(key, cancellationToken);
    }
}

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class;

    Task<T?> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, DistributedCacheEntryOptions? options = default,
        CancellationToken cancellationToken = default) where T : class;

    Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions? options = default, CancellationToken cancellationToken = default) where T : class;
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);
}