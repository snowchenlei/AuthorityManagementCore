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
            return await cache.GetStringAsync(key) != null;
        }

        /// <summary>
        /// 默认编码序列化值
        /// </summary>
        /// <typeparam name="TEntity">类型</typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static async Task SetAsync<TEntity>(this IDistributedCache cache, string key, TEntity value) where TEntity : class
        {
            
            await cache.SetAsync(key, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)));
        }

        /// <summary>
        /// 指定编码序列化值
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="encoding"></param>
        public static async Task SetAsync<TEntity>(this IDistributedCache cache, string key, TEntity value, Encoding encoding)
        {
            await cache.SetAsync(key, encoding.GetBytes(JsonConvert.SerializeObject(value)));
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        public static async Task<TEntity> GetAsync<TEntity>(this IDistributedCache cache, string key)
        {
            return JsonConvert.DeserializeObject<TEntity>(await cache.GetStringAsync(key));
        }
    }
}