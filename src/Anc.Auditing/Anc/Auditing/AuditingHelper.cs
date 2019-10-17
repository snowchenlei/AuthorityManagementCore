using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Anc.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Anc.Auditing
{
    public class AuditingHelper : IAuditingHelper, ITransientDependency
    {
        protected AncAuditingOptions Options;
        protected IAuditSerializer AuditSerializer;
        protected ILogger<AuditingHelper> Logger { get; }

        public AuditingHelper(
            IAuditSerializer auditSerializer
            , ILogger<AuditingHelper> logger
            , IOptions<AncAuditingOptions> options
            )
        {
            Logger = logger;
            Options = options.Value;
            AuditSerializer = auditSerializer;
        }

        public virtual bool ShouldSaveAudit(MethodInfo methodInfo, bool defaultValue = false)
        {
            if (methodInfo == null)
            {
                return false;
            }

            if (!methodInfo.IsPublic)
            {
                return false;
            }

            if (methodInfo.IsDefined(typeof(AuditedAttribute), true))
            {
                return true;
            }

            if (methodInfo.IsDefined(typeof(DisableAuditingAttribute), true))
            {
                return false;
            }

            var classType = methodInfo.DeclaringType;
            if (classType != null)
            {
                if (AuditingInterceptorRegistrar.ShouldAuditTypeByDefault(classType))
                {
                    return true;
                }
            }

            return defaultValue;
        }

        public virtual AuditLogInfo CreateAuditLogInfo()
        {
            var auditInfo = new AuditLogInfo
            {
                ApplicationName = Options.ApplicationName,
                UserId = null,//CurrentUser.Id,
                UserName = "",//CurrentUser.UserName,
                ClientId = "",//CurrentClient.Id,
                CorrelationId = Guid.NewGuid().ToString("N"),
                //ImpersonatorUserId = AbpSession.ImpersonatorUserId, //TODO: Impersonation system is not available yet!
                //ImpersonatorTenantId = AbpSession.ImpersonatorTenantId,
                ExecutionTime = DateTime.Now
            };

            return auditInfo;
        }

        public virtual AuditLogActionInfo CreateAuditLogAction(
            AuditLogInfo auditLog,
            Type type,
            MethodInfo method,
            object[] arguments)
        {
            return CreateAuditLogAction(auditLog, type, method, CreateArgumentsDictionary(method, arguments));
        }

        public virtual AuditLogActionInfo CreateAuditLogAction(
            AuditLogInfo auditLog,
            Type type,
            MethodInfo method,
            IDictionary<string, object> arguments)
        {
            var actionInfo = new AuditLogActionInfo
            {
                ServiceName = type != null
                    ? type.FullName
                    : "",
                MethodName = method.Name,
                Parameters = SerializeConvertArguments(arguments),
                ExecutionTime = DateTime.Now
            };

            //TODO Execute contributors

            return actionInfo;
        }

        protected virtual string SerializeConvertArguments(IDictionary<string, object> arguments)
        {
            try
            {
                if (arguments.IsNullOrEmpty())
                {
                    return "{}";
                }

                var dictionary = new Dictionary<string, object>();

                foreach (var argument in arguments)
                {
                    if (argument.Value != null && Options.IgnoredTypes.Any(t => t.IsInstanceOfType(argument.Value)))
                    {
                        dictionary[argument.Key] = null;
                    }
                    else
                    {
                        dictionary[argument.Key] = argument.Value;
                    }
                }

                return AuditSerializer.Serialize(dictionary);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, LogLevel.Warning);
                return "{}";
            }
        }

        protected virtual Dictionary<string, object> CreateArgumentsDictionary(MethodInfo method, object[] arguments)
        {
            var parameters = method.GetParameters();
            var dictionary = new Dictionary<string, object>();

            for (var i = 0; i < parameters.Length; i++)
            {
                dictionary[parameters[i].Name] = arguments[i];
            }

            return dictionary;
        }
    }
}