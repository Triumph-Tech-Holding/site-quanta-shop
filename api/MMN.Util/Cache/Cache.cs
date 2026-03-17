using Microsoft.Extensions.Caching.Memory;
using System;

namespace MMN.Util.Cache
{
    public class Cache : ICache
    {
        private readonly IMemoryCache _cache;
        public Cache(IMemoryCache cache)
        {
            _cache = cache;
        }

        public object GetItem(CacheKeys key)
        {
            return _cache.Get(key.ToString());
        }

        public object GetItem(string key)
        {
            return _cache.Get(key.ToString());
        }

        public void SetItem(CacheKeys key, object value)
        {
            _cache.Set(key.ToString(), value, DateTime.UtcNow.AddMinutes(30));
        }

        public void SetItem(string key, object value, DateTime? absoluteExpiration = null)
        {
            _cache.Set(key.ToString(), value, absoluteExpiration ?? DateTime.UtcNow.AddMinutes(30));
        }

        public void RemoveItem(CacheKeys key)
        {
            _cache.Remove(key.ToString());
        }

        public void RemoveItem(string key)
        {
            _cache.Remove(key.ToString());
        }
    }

    public enum CacheKeys
    {
        RootSite,
        GroupMenu,
        Configurations,
        Status,
        ArquivosProduto,
        Tipo,
        Cupom,
        Graduacao,
        Credenciamentos,
        Categorias
    }
}
