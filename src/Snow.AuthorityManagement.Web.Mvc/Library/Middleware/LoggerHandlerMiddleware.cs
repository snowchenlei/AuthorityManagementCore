using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Snow.AuthorityManagement.Web.Library.Middleware
{
    public class LoggerHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggerHandlerMiddleware(
            RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next(context);
        }
    }
}