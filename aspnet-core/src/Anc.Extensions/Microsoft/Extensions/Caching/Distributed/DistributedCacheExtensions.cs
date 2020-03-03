﻿using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Microsoft.Extensions.Caching.Distributed
{
    public static class DistributedCacheExtensions
    {
        /// <summary>
        /// 缓存是否存在
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool ExistsString(this IDistributedCache cache, string key)
        {
            ValidateCacheKey(key);

            return cache.GetString(key) == null;
        }

        /// <summary>
        /// 缓存是否存在
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<bool> ExistsStringAsync(this IDistributedCache cache, string key)
        {
            ValidateCacheKey(key);

            return await cache.GetStringAsync(key) != null;
        }


        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="TEntity">类型</typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static async Task SetAsync<TEntity>(this IDistributedCache cache, string key, TEntity value) where TEntity : class
        {
            ValidateCacheKey(key);

            string result = JsonConvert.SerializeObject(value);
            await cache.SetStringAsync(key, result);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        public static async Task<TEntity> GetAsync<TEntity>(this IDistributedCache cache, string key)
        {
            ValidateCacheKey(key);
            string value = await cache.GetStringAsync(key);
            if (value == null)
            {
                return default;
            }
            return JsonConvert.DeserializeObject<TEntity>(value);
        }

        /// <summary>
        /// 获取或设置缓存
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="factory">获取数据的委托</param>
        /// <returns></returns>
        public static async Task<TEntity> GetOrCreateAsync<TEntity>(this IDistributedCache cache, string key, Func<Task<TEntity>> factory) where TEntity : class
        {
            ValidateCacheKey(key);
            TEntity entity = await cache.GetAsync<TEntity>(key);
            if (entity != null)
            {
                return entity;
            }

            entity = await factory();
            await cache.SetAsync(key, entity);
            return entity;
        }

        private static void ValidateCacheKey(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
        }
    }
}