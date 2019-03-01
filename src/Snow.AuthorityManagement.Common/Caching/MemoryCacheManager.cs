using Microsoft.Extensions.Caching.Memory;
using System;
using System.Runtime.Caching;

namespace Cl.AuthorityManagement.Api.Caching
{
    public class MemoryCacheManager : ICacheService
    {
        public object this[string key]
        {
            get
            {
                return Get<object>(key);
            }
            set
            {
                if (!Contains(key))
                {
                    Set(key, value, 120);
                }
            }
        }

        private ObjectCache CurrentCache = MemoryCache.Default;

        /// <summary>
        /// 清除
        /// </summary>
        public void Clear()
        {
            foreach (var item in CurrentCache)
            {
                this.Remove(item.Key);
            }
        }

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>是否存在</returns>
        public bool Contains(string key)
        {
            return CurrentCache.Contains(key);
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="key">键值</param>
        /// <returns>数据</returns>
        public TEntity Get<TEntity>(string key)
        {
            return (TEntity)CurrentCache[key];
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="key">键值</param>
        public void Remove(string key)
        {
            CurrentCache.Remove(key);
        }
                
        public void Set(string key, object value, long cacheTime)
        {
            Set(key, value, DateTimeOffset.Now.AddSeconds(cacheTime));
        }

        /// <summary>
        /// 绝对过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="cacheTime"></param>
        public void Set(string key, object value, DateTimeOffset cacheTime)
        {
            CurrentCache.Set(key, value, new CacheItemPolicy() { AbsoluteExpiration = cacheTime });
        }

        /// <summary>
        /// 滑动过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="cacheTime"></param>
        public void Set(string key, object value, TimeSpan cacheTime)
        {
            CurrentCache.Set(key, value, new CacheItemPolicy() { SlidingExpiration = cacheTime });
        }

        /// <summary>
        /// 变化监控
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="value">数据</param>
        /// <param name="changeMonitor">变化监控</param>
        public void Set(string key, object value, ChangeMonitor changeMonitor)
        {
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.ChangeMonitors.Add(changeMonitor);
            CurrentCache.Set(key, value, policy);
        }
    }
}
