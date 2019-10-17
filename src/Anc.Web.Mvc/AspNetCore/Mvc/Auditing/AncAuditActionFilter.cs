using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Anc.AspNetCore.AspNetCore.Configuration;
using Anc.AspNetCore.Web.Mvc.Extensions;
using Anc.Auditing;
using log4net.Core;
using log4net.Layout;
using log4net.Layout.Pattern;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Anc.AspNetCore.AspNetCore.Mvc.Auditing
{
    public class AncAuditActionFilter : IAsyncActionFilter, ITransientDependency
    {
        protected AncAuditingOptions Options { get; }
        private readonly IAuditingHelper _auditingHelper;
        private readonly IAuditingManager _auditingManager;

        public AncAuditActionFilter(
            IOptions<AncAuditingOptions> options
            , IAuditingHelper auditingHelper
            , IAuditingManager auditingManager
            )
        {
            Options = options.Value;

            _auditingHelper = auditingHelper;
            _auditingManager = auditingManager;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!ShouldSaveAudit(context, out var auditLog, out var auditLogAction))
            {
                await next();
                return;
            }
            var stopwatch = Stopwatch.StartNew();

            try
            {
                var result = await next();

                if (result.Exception != null && !result.ExceptionHandled)
                {
                    auditLog.Exceptions.Add(result.Exception);
                }
            }
            catch (Exception ex)
            {
                auditLog.Exceptions.Add(ex);
                throw;
            }
            finally
            {
                stopwatch.Stop();
                auditLogAction.ExecutionDuration = Convert.ToInt32(stopwatch.Elapsed.TotalMilliseconds);
                auditLog.Actions.Add(auditLogAction);
            }
        }

        private bool ShouldSaveAudit(ActionExecutingContext context, out AuditLogInfo auditLog, out AuditLogActionInfo auditLogAction)
        {
            auditLog = null;
            auditLogAction = null;

            if (!Options.IsEnabled)
            {
                return false;
            }

            if (!context.ActionDescriptor.IsControllerAction())
            {
                return false;
            }

            AuditLogInfo auditLogCache = _auditingManager.Current;
            if (auditLogCache == null)
            {
                return false;
            }

            if (!_auditingHelper.ShouldSaveAudit(context.ActionDescriptor.GetMethodInfo(), true))
            {
                return false;
            }

            //TODO: This is partially duplication of AuditHelper.ShouldSaveAudit method. Check why it does not work for controllers
            if (!AuditingInterceptorRegistrar.ShouldAuditTypeByDefault(context.Controller.GetType()))
            {
                return false;
            }

            auditLog = auditLogCache;
            auditLogAction = _auditingHelper.CreateAuditLogAction(
                auditLog,
                context.ActionDescriptor.AsControllerActionDescriptor().ControllerTypeInfo.AsType(),
                context.ActionDescriptor.AsControllerActionDescriptor().MethodInfo,
                context.ActionArguments
            );

            return true;
        }
    }
}