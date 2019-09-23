using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Snow.AuthorityManagement.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.Web.Library
{
    public class ModelStateInvalidFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            List<string> errors = new List<string>();

            if (!filterContext.ModelState.IsValid)
            {
                foreach (var item in filterContext.ModelState.Values)
                {
                    foreach (var error in item.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                filterContext.HttpContext.Response.StatusCode = 400;
                filterContext.Result = new ContentResult
                {
                    Content = "[" + string.Join(",", errors) + "]"
                };
            }
        }
    }
}