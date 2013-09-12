using System;
using Microsoft.ApplicationServer.Caching;
using WebAPI.OutputCache.Cache;

namespace Strathweb.CacheOutput.Azure
{
    public class AzureCachingProvider : IApiOutputCache
    {
        private readonly DataCache _cache;

        public AzureCachingProvider()
        {
            _cache = new DataCache();
        }

        public void RemoveStartsWith(string key)
        {
            _cache.Remove(key);
        }

        public T Get<T>(string key) where T : class
        {
            var result = _cache.Get(key) as T;
            return result;
        }

        public object Get(string key)
        {
            var result = _cache.Get(key);
            return result;
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }

        public bool Contains(string key)
        {
            var result = _cache.Get(key);
            return result != null;
        }

        public void Add(string key, object o, DateTimeOffset expiration, string dependsOnKey = null)
        {
            var exp = expiration - DateTime.Now;
            _cache.Put(key, o, exp);
        }
    }
}
