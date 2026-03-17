using System;
using Microsoft.Extensions.Caching.Memory;

public class CacheManager
{
    private readonly IMemoryCache _cache;

    public CacheManager(IMemoryCache cache)
    {
        _cache = cache;
    }

    public void SetItem(string key, object value)
    {
        var expiration = GetExpirationTime();
        _cache.Set(key, value, expiration);
    }

    public object GetItem(string key)
    {
        return _cache.TryGetValue(key, out var value) ? value : null;
    }

    private TimeSpan GetExpirationTime()
    {
        var now = DateTime.Now;
        var nextRunTime = now.Date.AddHours(2);

        if (now >= nextRunTime)
        {
            nextRunTime = nextRunTime.AddDays(1);
        }

        return nextRunTime - now;
    }
}
