using System;

namespace Snow.AuthorityManagement.Common.Conversion
{
    public sealed class ConvertHelper
    {
        /// <summary>
        /// 类型转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">需要转换的值</param>
        /// <returns></returns>
        public static T ConvertType<T>(object obj)
        {
            try
            {
                T t = (T)Convert.ChangeType(obj, typeof(T));
                return t;
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        /// <summary>
        /// 类型转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">需要转换的值</param>
        /// <param name="defaultVal">期望默认值</param>
        /// <returns></returns>
        public static T ConvertType<T>(object obj, T defaultVal)
        {
            if (obj == null)
            {
                return defaultVal;
            }
            try
            {
                T t = (T)Convert.ChangeType(obj, typeof(T));
                return t;
            }
            catch (Exception ex)
            {
                return defaultVal;
            }
        }
    }
}
