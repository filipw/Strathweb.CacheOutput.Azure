using System;
using Microsoft.ApplicationServer.Caching;
using WebAPI.OutputCache.Cache;

namespace Strathweb.CacheOutput.Azure
{
    public class AzureCachingProvider : IApiOutputCache
    {
        private readonly DataCache _cache;
        private const string Region = "GlobalRegion";

        public AzureCachingProvider()
        {
            _cache = new DataCache();
            _cache.CreateRegion(Region);
        }

        public void RemoveStartsWith(string key)
        {
            var objs = _cache.GetObjectsByTag(new DataCacheTag(key), Region);
            foreach (var o in objs)
            {
                _cache.Remove(o.Key, Region);
            }
        }

        public T Get<T>(string key) where T : class
        {
            var result = _cache.Get(key, Region) as T;
            return result;
        }

        public object Get(string key)
        {
            var result = _cache.Get(key, Region);
            return result;
        }

        public void Remove(string key)
        {
            _cache.Remove(key, Region);
        }

        public bool Contains(string key)
        {
            var result = _cache.Get(key, Region);
            if (result != null) return true;

            return false;
        }

        public void Add(string key, object o, DateTimeOffset expiration, string dependsOnKey = null)
        {
            var exp = expiration - DateTime.Now;
            if (dependsOnKey == null)
            {
                dependsOnKey = key;
            }

            _cache.Put(key, o, exp, new[] { new DataCacheTag(dependsOnKey) }, Region);
        }
    }
}
