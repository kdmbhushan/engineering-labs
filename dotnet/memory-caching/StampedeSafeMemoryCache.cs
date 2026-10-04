using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Memory;

namespace EngineeringLabs.MemoryCaching;

public sealed class StampedeSafeMemoryCache
{
    private readonly IMemoryCache _cache;
    private readonly ConcurrentDictionary<string, SemaphoreSlim> _locks = new();

    public StampedeSafeMemoryCache(IMemoryCache cache)
    {
        _cache = cache;
    }

    public async Task<T> GetOrCreateAsync<T>(
        string key,
        Func<CancellationToken, Task<T>> factory,
        TimeSpan absoluteExpirationRelativeToNow,
        CancellationToken ct = default)
    {
        if (_cache.TryGetValue(key, out T? cachedValue) && cachedValue is not null)
        {
            return cachedValue;
        }

        var keyLock = _locks.GetOrAdd(key, _ => new SemaphoreSlim(1, 1));
        await keyLock.WaitAsync(ct);

        try
        {
            // Re-check after acquiring the lock (Double-Check pattern)
            if (_cache.TryGetValue(key, out cachedValue) && cachedValue is not null)
            {
                return cachedValue;
            }

            var value = await factory(ct);
            _cache.Set(key, value, absoluteExpirationRelativeToNow);
            return value;
        }
        finally
        {
            keyLock.Release();
        }
    }
}
