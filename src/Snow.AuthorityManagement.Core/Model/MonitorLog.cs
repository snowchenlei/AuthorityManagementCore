using System;
using System.Collections.Generic;
using System.Text;

namespace Snow.AuthorityManagement.Core.Model
{
    /// <summary>
    /// 监控日志对象
    /// </summary>
    public class MonitorLog
    {
        public string Url { get; set; }

        public DateTime ExecuteStartTime { get; set; }
        public DateTime ExecuteEndTime { get; set; }

        /// <summary>
        /// 请求的Action 参数(JSON)
        /// </summary>
        public string ActionParams { get; set; }

        /// <summary>
        /// Http请求头
        /// </summary>
        public string HttpRequestHeaders { get; set; }

        /// <summary>
        /// 请求方式
        /// </summary>
        public string HttpMethod { get; set; }

        /// <summary>
        /// 响应数据
        /// </summary>
        public string ResponseData { get; set; }

        /// <summary>
        /// 请求的IP地址
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 获取监控指标日志
        /// </summary>
        /// <param name="mtype"></param>
        /// <returns></returns>
        public string GetLoginfo()
        {
            return $@" 客户端【{IP}】以【{HttpRequestHeaders}】头使用【{HttpMethod}】携带【{ActionParams}】请求【{Url}】响应【{ResponseData}】";
            //return $@"
            //Action执行时间监控：
            //Url：{Url}
            //开始时间：{ExecuteStartTime}
            //结束时间：{ExecuteEndTime}
            //总 时 间：{(ExecuteEndTime - ExecuteStartTime).TotalSeconds}秒
            //Request参数：{ActionParams}
            //Response参数：{ResponseData}
            //Http请求头:{HttpRequestHeaders}
            //HttpMethod:{HttpMethod}";
        }

        public string GetErrorInfo()
        {
            return $@" 客户端【{IP}】以【{HttpRequestHeaders}】头使用【{HttpMethod}】携带【{ActionParams}】请求【{Url}】产生异常：";
        }

        /// <summary>
        /// 获取Action 参数
        /// </summary>
        /// <param name="Collections"></param>
        /// <returns></returns>
        public string GetCollections(IDictionary<string, object> Collections)
        {
            string Parameters = string.Empty;
            if (Collections == null || Collections.Count == 0)
            {
                return Parameters;
            }
            foreach (string key in Collections.Keys)
            {
                Parameters += string.Format("{0}={1}&", key, Collections[key]);
            }
            if (!string.IsNullOrWhiteSpace(Parameters) && Parameters.EndsWith("&"))
            {
                Parameters = Parameters.Substring(0, Parameters.Length - 1);
            }
            return Parameters;
        }
    }
}