using System;

namespace MMN.Util.Cache
{
    public interface ICache
    {
        object GetItem(CacheKeys key);
        object GetItem(string key);
        void SetItem(CacheKeys key, object value);
        void SetItem(string key, object value, DateTime? absoluteExpiration = null);
        void RemoveItem(CacheKeys key);
        void RemoveItem(string key);
    }
}
