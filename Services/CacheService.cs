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

    public async Task<T?> GetOrCreateAsync<T>(string key, Func<DistributedCacheEntryOptions, Task<T>> factory,
        CancellationToken cancellationToken = default) where T : class
    {
        var cacheOptions = new DistributedCacheEntryOptions();
        var value = await GetAsync<T>(key, cancellationToken);
        if (value is not null) return value;
        value = await factory(cacheOptions);
        await SetAsync(key, value, cacheOptions, cancellationToken);
        return value;
    }

    public async Task SetAsync<T>(string key, T value,
        CancellationToken cancellationToken = default)
        where T : class
    {
        var cacheValue = JsonSerializer.Serialize(value);
        await _distributedCache.SetStringAsync(key, cacheValue, cancellationToken);
    }

    public async Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions options,
        CancellationToken cancellationToken = default)
        where T : class
    {
        var cacheValue = JsonSerializer.Serialize(value);
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

    Task<T?> GetOrCreateAsync<T>(string key, Func<DistributedCacheEntryOptions, Task<T>> factory,
        CancellationToken cancellationToken = default) where T : class;

    Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions options,
        CancellationToken cancellationToken = default) where T : class;

    Task SetAsync<T>(string key, T value,
        CancellationToken cancellationToken = default) where T : class;

    Task RemoveAsync(string key, CancellationToken cancellationToken = default);
}