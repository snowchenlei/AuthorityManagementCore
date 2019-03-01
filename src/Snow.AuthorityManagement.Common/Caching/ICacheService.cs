using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cl.AuthorityManagement.Api.Caching
{
    public interface ICacheService
    {
        object this[string key] { get; set; }

        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="key">键值</param>
        /// <returns>数据</returns>
        TEntity Get<TEntity>(string key);

        /// <summary>
        /// 滑动过期时间
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="value">数据</param>
        /// <param name="cacheTime">滑动缓存时间</param>
        void Set(string key, object value, TimeSpan cacheTime);

        /// <summary>
        /// 绝对过期时间
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="value">数据</param>
        /// <param name="cacheTime">绝对过期时间(S)</param>
        void Set(string key, object value, long cacheTime);

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>是否存在</returns>
        bool Contains(string key);

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="key">键值</param>
        void Remove(string key);

        /// <summary>
        /// 清除
        /// </summary>
        void Clear();
    }
}
