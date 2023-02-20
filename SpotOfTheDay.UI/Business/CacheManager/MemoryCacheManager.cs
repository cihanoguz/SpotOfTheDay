using System;
using Microsoft.Extensions.Caching.Memory;

namespace SpotOfTheDay.UI.Business.CacheManager
{
    public class MemoryCacheManager : ICacheManager
    {
        private IMemoryCache _memoryCache;

        public MemoryCacheManager(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public void Add(string key, object value, int duration)
        {
            _memoryCache.Set(key, value, TimeSpan.FromMinutes(duration));
        }

        public object Get(string key)
        {
            return _memoryCache.Get(key);
        }

        public bool IsAdd(string key)
        {
            return _memoryCache.TryGetValue(key, out _);
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

    }
}

