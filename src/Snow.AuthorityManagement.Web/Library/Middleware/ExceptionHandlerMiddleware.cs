using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Snow.AuthorityManagement.Core.Exception;
using Snow.AuthorityManagement.Core.Model;

namespace Snow.AuthorityManagement.Web.Library.Middleware
{
    /// <summary>
    /// 自定义异常处理中间件
    /// </summary>
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next
            , ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<ExceptionHandlerMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            MonitorLog monitorLog = new MonitorLog();
            try
            {
                HttpRequest request = context.Request;
                monitorLog.ExecuteStartTime = DateTime.Now;
                monitorLog.HttpMethod = request.Method;
                monitorLog.ActionParams = await ReadBodyAsync(context.Request);
                monitorLog.HttpRequestHeaders = JsonConvert.SerializeObject(request.Headers);
                monitorLog.IP = GetUserIp(context);

                await _next(context);
                monitorLog.ExecuteEndTime = DateTime.Now;
                monitorLog.ResponseData = await ReadBodyAsync(context.Response);
                _logger.LogInformation(monitorLog.GetLoginfo());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, monitorLog.GetErrorInfo());
                var statusCode = context.Response.StatusCode;
                if (ex is UserFriendlyException)
                {
                    statusCode = 400;
                }
                else if (ex is AncAuthorizationException)
                {
                    statusCode = 401;
                }
                else if (ex is AncAuthenticationException)
                {
                    statusCode = 403;
                }
                await HandleExceptionAsync(context, statusCode, ex.Message);
            }
            //finally
            //{
            //    var statusCode = context.Response.StatusCode;
            //    var msg = "";
            //    switch (statusCode)
            //    {
            //        case 500:
            //            msg = "服务器系统内部错误";
            //            break;

            //        case 401:
            //            msg = "未登录";
            //            break;

            //        case 403:
            //            msg = "无权限执行此操作";
            //            break;

            //        case 408:
            //            msg = "请求超时";
            //            break;
            //    }
            //    if (!string.IsNullOrWhiteSpace(msg))
            //    {
            //        await HandleExceptionAsync(context, statusCode, msg);
            //    }
            //}
        }

        public static string GetUserIp(HttpContext context)
        {
            var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = context.Connection.RemoteIpAddress.ToString();
            }
            return ip;
        }

        private async Task<string> ReadBodyAsync(HttpResponse response)
        {
            if (response.Body.CanSeek)
            {
                if (response.ContentLength > 0)
                {
                    response.Body.Seek(0, SeekOrigin.Begin);
                    var encoding = GetEncoding(response.ContentType);
                    return await this.ReadStreamAsync(response.Body, encoding).ConfigureAwait(false);
                    //response.Body.Position = position;
                    //读取完成后再重新赋值位置这个过程可能不需要，因为数据流是只写的
                }
            }

            return String.Empty;
        }

        private async Task<string> ReadBodyAsync(HttpRequest request)
        {
            if (request.ContentLength > 0)
            {
                await EnableRewindAsync(request).ConfigureAwait(false);
                var encoding = GetEncoding(request.ContentType);
                return await this.ReadStreamAsync(request.Body, encoding).ConfigureAwait(false);
            }
            return String.Empty;
        }

        private Encoding GetEncoding(string contentType)
        {
            var requestMediaType = contentType == null ? default(MediaType) : new MediaType(contentType);
            var requestEncoding = requestMediaType.Encoding;
            if (requestEncoding == null)
            {
                requestEncoding = Encoding.UTF8;
            }
            return requestEncoding;
        }

        private async Task EnableRewindAsync(HttpRequest request)
        {
            if (!request.Body.CanSeek)
            {
                request.EnableBuffering();

                await request.Body.DrainAsync(CancellationToken.None);
                request.Body.Seek(0L, SeekOrigin.Begin);
            }
        }

        private async Task<string> ReadStreamAsync(Stream stream, Encoding encoding)
        {
            using (StreamReader sr = new StreamReader(stream, encoding, true, 1024, true))//这里注意Body部分不能随StreamReader一起释放
            {
                var str = await sr.ReadToEndAsync();
                stream.Seek(0, SeekOrigin.Begin);//内容读取完成后需要将当前位置初始化，否则后面的InputFormatter会无法读取
                return str;
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, int statusCode, string msg)
        {
            try
            {
                context.Response.ContentType = "application/json;charset=utf-8";
                context.Response.StatusCode = statusCode;
                await context.Response.WriteAsync(msg);
            }
            catch (Exception ex)
            {
            }
        }
    }
}