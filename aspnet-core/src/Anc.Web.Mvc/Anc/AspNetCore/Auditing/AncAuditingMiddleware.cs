using System;
using System.Threading.Tasks;
using Anc.Auditing;
using Anc.DependencyInjection;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Anc.AspNetCore.Auditing
{
    public class AncAuditingMiddleware : IMiddleware, ITransientDependency
    {
        private readonly IAuditingManager _auditingManager;

        protected AncAuditingOptions Options { get; }

        public AncAuditingMiddleware(IAuditingManager auditingManager
            , IOptions<AncAuditingOptions> options
            )
        {
            Options = options.Value;
            _auditingManager = auditingManager;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (!ShouldWriteAuditLog(context))
            {
                await next(context);
                return;
            }
            using (var scope = _auditingManager.BeginScope())
            {
                try
                {
                    await next(context);
                }
                finally
                {
                    await scope.SaveAsync();
                }
            }
        }

        private bool ShouldWriteAuditLog(HttpContext httpContext)
        {
            if (!Options.IsEnabled)
            {
                return false;
            }
            // TODO:用户判断
            //if (!Options.IsEnabledForAnonymousUsers && !_session.IsAuthenticated)
            //{
            //    return false;
            //}

            if (!Options.IsEnabledForGetRequests &&
                string.Equals(httpContext.Request.Method, HttpMethods.Get, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }
    }
}