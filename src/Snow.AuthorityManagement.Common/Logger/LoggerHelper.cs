using Snow.AuthorityManagement.Enum;
using Snow.AuthorityManagement.Model.Logger;
using log4net;
using System;

namespace Snow.AuthorityManagement.Common.Logger
{
    public class LoggerHelper
    {
        private static readonly ILog LogApiError = LogManager.GetLogger("s","LogApiError");
        private static readonly ILog LogMvcError = LogManager.GetLogger("s", "LogMvcError");

        private static readonly ILog LogRequestMonitor = LogManager.GetLogger("s", "LogRequestMonitor");
        private static readonly ILog LogApiMonitor = LogManager.GetLogger("s", "LogApiMonitor");
        private static readonly ILog LogMvcMonitor = LogManager.GetLogger("s", "LogMvcMonitor");

        private static readonly ILog LogOperate = LogManager.GetLogger("s", "LogOperate");

        #region 异常
        /// <summary>
        /// Api异常
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <param name="ex"></param>
        public static void ApiError(object obj, Exception ex = null)
        {
            if (obj is MonitorLog)
            {
                MonitorLog monLog = obj as MonitorLog;
                string innerException = String.Empty;
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                    innerException += ex.InnerException?.Message + "\r\n" + ex.InnerException?.StackTrace + "\r\n";
                }
                string message = monLog.GetErrorInfo() +
                    $@"【{ex.Message}】内部错误【{ex.InnerException?.Message}】";
                LogApiError.Error(GetError(monLog, message, innerException, (int)ApplicationType.Api), ex);
            }
            else
            {
                LogApiError.Error(obj, ex);
            }
        }

        /// <summary>
        /// Mvc异常
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <param name="ex"></param>
        public static void MvcError(object obj, Exception ex = null)
        {
            if(obj is MonitorLog)
            {
                MonitorLog monLog = obj as MonitorLog;
                string innerException = String.Empty;
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                    innerException += ex.InnerException?.Message + "\r\n" + ex.InnerException?.StackTrace + "\r\n";
                }
                string message = monLog.GetErrorInfo() +
                    $@"【{ex.Message}】内部错误【{ex.InnerException?.Message}】";
                var s = GetError(monLog, message, innerException, (int)ApplicationType.Mvc);
                LogMvcError.Error(s, ex);
            }
            else
            {
                LogMvcError.Error(obj, ex);
            }            
        }
        #endregion

        private static object GetError(MonitorLog monLog, string message, 
            string innerException, int applicationType)
        {
            return new
            {
                Controller = monLog.ControllerName,
                Action = monLog.ActionName,
                IP = monLog.IP,
                Head = monLog.HttpRequestHeaders,
                HttpMethod = monLog.HttpMethod,
                RequestMessage = monLog.ActionParams,
                ApplicationType = applicationType,
                InnerException = innerException,
                Description = message
            };
        }

        #region 监控
        /// <summary>
        /// Api监控
        /// </summary>
        /// <param name="message"></param>
        public static void ApiMonitor(object obj)
        {
            if (obj is MonitorLog)
            {
                MonitorLog monLog = obj as MonitorLog;
                LogApiMonitor.Info(GetMonitor(monLog, (int)ApplicationType.Api));
            }
            else {
                LogApiMonitor.Info(obj);
            }
        }
        
        /// <summary>
        /// Mvc监控
        /// </summary>
        /// <param name="message"></param>
        public static void MVcMonitor(object obj)
        {
            if (obj is MonitorLog)
            {
                MonitorLog monLog = obj as MonitorLog;
                LogMvcMonitor.Info(GetMonitor(monLog, (int)ApplicationType.Mvc));
            }
            else
            {
                LogMvcMonitor.Info(obj);
            }
        } 

        private static object GetMonitor(MonitorLog monLog, int applicationType)
        {
            return new
            {
                Controller = monLog.ControllerName,
                Action = monLog.ActionName,
                StartTime = monLog.ExecuteStartTime,
                EndTime = monLog.ExecuteEndTime,
                SumTime = (monLog.ExecuteEndTime - monLog.ExecuteStartTime).TotalSeconds,
                IP = monLog.IP,
                HttpMethod = monLog.HttpMethod,
                Head = monLog.HttpRequestHeaders,
                RequestMessage = monLog.ActionParams,
                ResponseMessage = monLog.ResponseData,
                ApplicationType = applicationType,
                Description = monLog.GetLoginfo()
            };
        }
        
        /// <summary>
        /// 请求日志
        /// </summary>
        /// <param name="message"></param>
        public static void RequestMonitor(string message)
        {
            string[] messages = message.TrimEnd(']').Split('[');
            string[] info = messages[1].Split('；');
            //LogRequestMonitor.Info(message);
            LogRequestMonitor.Info(new
            {
                Head = messages[0],
                Url = info[0].Split('：')[1],
                Message = message,
                RequestMessage = info[1].Split('：')[1],
                ResponseMessage = info[2].Split('：')[1],
            });
        }
        #endregion

        public static void Operate(object obj)
        {
            LogOperate.Info(obj);
        }
    }
}
