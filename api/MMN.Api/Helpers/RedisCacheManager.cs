using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;

namespace MMN.Api.Helpers
{
    public class RedisCacheManager
    {
        private readonly IDistributedCache _cache;

        public RedisCacheManager(IDistributedCache cache)
        {
            _cache = cache;
        }

        public void SetItem(string key, object value)
        {
            var expiration = GetExpirationTime();
            var serializedValue = JsonConvert.SerializeObject(value);
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            };
            _cache.SetString(key, serializedValue, options);
        }

        public T GetItem<T>(string key)
        {
            var serializedValue = _cache.GetString(key);
            return serializedValue == null ? default : JsonConvert.DeserializeObject<T>(serializedValue);
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
}
