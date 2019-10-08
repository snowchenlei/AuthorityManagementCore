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
using Anc.Dependency;
using log4net.Core;
using log4net.Layout;
using log4net.Layout.Pattern;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Anc.AspNetCore.AspNetCore.Mvc.Auditing
{
    public class AncAuditActionFilter : IAsyncActionFilter, ITransientDependency
    {
        private readonly IAuditingHelper _auditingHelper;
        private readonly ILogger<AncAuditActionFilter> _logger;
        private readonly IAncAspNetCoreConfiguration _configuration;

        public AncAuditActionFilter(IAuditingHelper auditingHelper
            , ILogger<AncAuditActionFilter> logger)
        {
            _logger = logger;
            _auditingHelper = auditingHelper;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!ShouldSaveAudit(context))
            {
                await next();
                return;
            }
            var auditInfo = _auditingHelper.CreateAuditInfo(
                context.ActionDescriptor.AsControllerActionDescriptor().ControllerTypeInfo.AsType(),
                context.ActionDescriptor.AsControllerActionDescriptor().MethodInfo,
                context.ActionArguments
            );

            var stopwatch = Stopwatch.StartNew();

            try
            {
                var result = await next();
                if (result.Exception != null && !result.ExceptionHandled)
                {
                    auditInfo.Exception = result.Exception;
                }
            }
            catch (Exception ex)
            {
                auditInfo.Exception = ex;
                throw;
            }
            finally
            {
                stopwatch.Stop();
                auditInfo.ExecutionDuration = Convert.ToInt32(stopwatch.Elapsed.TotalMilliseconds);
                _logger.LogInformation("asdas");
                await _auditingHelper.SaveAsync(auditInfo);
            }
        }

        private bool ShouldSaveAudit(ActionExecutingContext actionContext)
        {
            return _configuration.IsAuditingEnabled &&
                   actionContext.ActionDescriptor.IsControllerAction() &&
                   _auditingHelper.ShouldSaveAudit(actionContext.ActionDescriptor.GetMethodInfo(), true);
        }
    }

    public class CustomerPatternLayout : PatternLayout
    {
        public CustomerPatternLayout()
        {
            this.AddConverter("Property", typeof(CustomerPatternConvert));
        }
    }

    public class CustomerPatternConvert : PatternLayoutConverter
    {
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            if (Option != null)
            {
                WriteObject(writer, loggingEvent.Repository, LookupProperty(Option, loggingEvent));
            }
            else
            {
                WriteDictionary(writer, loggingEvent.Repository, loggingEvent.GetProperties());
            }
        }

        private object LookupProperty(string property, LoggingEvent loggingEvent)
        {
            object propertyValue = String.Empty;
            PropertyInfo propertyInfo = loggingEvent.MessageObject.GetType().GetProperty(property);
            if (propertyInfo != null)
            {
                propertyValue = propertyInfo.GetValue(loggingEvent.MessageObject, null);
            }
            return propertyValue;
        }
    }
}