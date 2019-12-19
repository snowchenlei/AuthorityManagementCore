using Microsoft.AspNetCore.Builder;

namespace Snow.AuthorityManagement.Web.Library.Middleware
{
    public static class ExceptionHandlerExtensions
    {
        public static IApplicationBuilder UseCustomerExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}