using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.Common.Conversion
{
    public static class Serialization
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>成功后的Json数据</returns>
        public static string SerializeObject(Object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">模型类</typeparam>
        /// <param name="value">json数据</param>
        /// <returns>对应的模型数据</returns>
        public static T DeserializeObject<T>(JToken jToken)
        {
            return JsonConvert.DeserializeObject<T>(jToken?.ToString());
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">模型类</typeparam>
        /// <param name="value">json数据</param>
        /// <returns>对应的模型数据</returns>
        public static T DeserializeObject<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }        
    }
}
