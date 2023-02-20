using System;
namespace SpotOfTheDay.UI.Business.CacheManager
{
    public interface ICacheManager
    {
        void Add(string key, object value, int duration);
        bool IsAdd(string key);
        object Get(string key);
        void Remove(string key);

    }
}

