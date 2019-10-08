using System;
using Anc.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Snow.AuthorityManagement.Core;
using Snow.AuthorityManagement.Core.Exception;

namespace Snow.AuthorityManagement.Web.Library
{
    /// <summary>
    /// Mvc异常处理
    /// </summary>
    public class AncExceptionAttribute : ExceptionFilterAttribute
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IModelMetadataProvider _modelMetadataProvider;

        public AncExceptionAttribute(
            IWebHostEnvironment hostingEnvironment,
            IModelMetadataProvider modelMetadataProvider)
        {
            _hostingEnvironment = hostingEnvironment;
            _modelMetadataProvider = modelMetadataProvider;
        }

        public override void OnException(ExceptionContext filterContext)
        {
            if (!_hostingEnvironment.IsDevelopment())
            {
                return;
            }
            HttpRequest request = filterContext.HttpContext.Request;
            Exception exception = filterContext.Exception;
            //异常是否处理
            if (filterContext.ExceptionHandled == true)
            {
                return;
            }
            if (exception is UserFriendlyException)
            {
                //filterContext.Result = new ApplicationErrorResult
                filterContext.HttpContext.Response.StatusCode = 400;
                filterContext.HttpContext.Response.WriteAsync(exception.Message);
            }
            //MonitorLog MonLog = null;
            //if (request.HttpContext.Items.ContainsKey("_thisWebApiOnActionMonitorLog_"))
            //{
            //    MonLog = request.HttpContext.Items["_thisWebApiOnActionMonitorLog_"] as MonitorLog;
            //}
            //else
            //{
            //    //获取Action 参数
            //    MonLog = new MonitorLog()
            //    {
            //        HttpRequestHeaders = request.Headers.ToString(),
            //        HttpMethod = request.Method,
            //        //IP = IPHelper.GetRealIP(),
            //        ExecuteEndTime = DateTime.Now,
            //        ActionName = filterContext.RouteData.Values["action"] as string,
            //        ControllerName = filterContext.RouteData.Values["controller"] as string,
            //        ResponseData = exception.Message
            //        //ActionParams = filterContext.HttpContext.Request.
            //    };
            //}

            //加入队列
            //    Resource.MvcErrorQueue.Enqueue(new KeyValuePair<Exception, object>(exception, MonLog));
            //    HttpException httpException = new HttpException(null, exception);

            // /*
            // * 1、根据对应的HTTP错误码跳转到错误页面
            // * 2、先对Action方法里引发的HTTP 404/400错误进行捕捉和处理
            // * 3、其他错误默认为HTTP 500服务器错误
            // */ if (httpException != null && (httpException.GetHttpCode() == 400 ||
            // httpException.GetHttpCode() == 404)) { filterContext.HttpContext.Response.StatusCode =
            // 404; filterContext.HttpContext.Response.WriteFile("~/HttpError/404.html"); } else {
            // filterContext.HttpContext.Response.StatusCode = 500;
            // filterContext.HttpContext.Response.WriteFile("~/HttpError/500.html"); }

            // //设置异常已经处理,否则会被其他异常过滤器覆盖 filterContext.ExceptionHandled = true;

            // //在派生类中重写时，获取或设置一个值，该值指定是否禁用IIS自定义错误。
            // filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }
    }
}