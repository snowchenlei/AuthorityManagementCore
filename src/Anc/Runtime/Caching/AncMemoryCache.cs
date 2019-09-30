using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Caching.Memory;

namespace Anc.Runtime.Caching
{
    /// <summary>
    /// MemoryCache实现版
    /// </summary>
    public class AncMemoryCache : ICache
    {
        private readonly IMemoryCache _memoryCache;

        public AncMemoryCache(IMemoryCache cache)
        {
            _memoryCache = cache;
        }

        public object Get(object key)
        {
            return _memoryCache.Get(key);
        }

        public TItem Get<TItem>(object key)
        {
            return _memoryCache.Get<TItem>(key);
        }

        public TItem Set<TItem>(object key, TItem value)
        {
            return _memoryCache.Set(key, value);
        }

        public TItem Set<TItem>(object key, TItem value, DateTimeOffset absoluteExpiration)
        {
            return _memoryCache.Set(key, value, absoluteExpiration);
        }

        public TItem Set<TItem>(object key, TItem value, TimeSpan absoluteExpirationRelativeToNow)
        {
            return _memoryCache.Set(key, value, absoluteExpirationRelativeToNow);
        }

        public bool TryGetValue<TItem>(object key, out TItem value)
        {
            return _memoryCache.TryGetValue(key, out value);
        }

        public void Dispose()
        {
            _memoryCache.Dispose();
        }
    }
}