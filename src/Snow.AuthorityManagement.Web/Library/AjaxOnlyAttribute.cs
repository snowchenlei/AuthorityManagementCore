using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Snow.AuthorityManagement.Web.Library
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AjaxOnlyAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 只支持Ajax访问
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //if (!filterContext.HttpContext.Request.IsAjaxRequest())
            //{
            //    filterContext.Result = new HttpNotFoundResult();
            //}
            base.OnActionExecuting(filterContext);
        }
    }
}