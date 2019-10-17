using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Transactions;
using Anc.DependencyInjection;
using Anc.Domain.Uow;
using Anc.Runtime.Session;
using Microsoft.Extensions.Logging;

namespace Anc.Auditing
{
    public class AuditingHelper : IAuditingHelper, ITransientDependency
    {
        private IAncSession AncSession { get; set; }

        private readonly IUnitOfWork _unitOfWork;
        private readonly IAncSession _session;
        private readonly ILogger<AuditingHelper> _logger;
        private readonly IAuditSerializer _auditSerializer;
        private readonly IAuditingConfiguration _configuration;
        private readonly IClientInfoProvider _clientInfoProvider;
        //private readonly IRepository<AuditLog, long> _auditLogRepository;

        public AuditingHelper(IUnitOfWork unitOfWork
            , IAncSession session
            , ILogger<AuditingHelper> logger
            , IAuditSerializer auditSerializer
            , IAuditingConfiguration configuration
            , IClientInfoProvider clientInfoProvider
            )
        {
            _logger = logger;
            _session = session;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _auditSerializer = auditSerializer;
            _clientInfoProvider = clientInfoProvider;
        }

        public bool ShouldSaveAudit(MethodInfo methodInfo, bool defaultValue = false)
        {
            if (!_configuration.IsEnabled)
            {
                return false;
            }

            if (!_configuration.IsEnabledForAnonymousUsers && (_session?.UserId == null))
            {
                return false;
            }

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
                if (classType.GetTypeInfo().IsDefined(typeof(AuditedAttribute), true))
                {
                    return true;
                }

                if (classType.GetTypeInfo().IsDefined(typeof(DisableAuditingAttribute), true))
                {
                    return false;
                }

                //if (_configuration.Selectors.Any(selector => selector.Predicate(classType)))
                //{
                //    return true;
                //}
            }

            return defaultValue;
        }

        public AuditInfo CreateAuditInfo(Type type, MethodInfo method, IDictionary<string, object> arguments)
        {
            var auditInfo = new AuditInfo
            {
                UserId = _session.UserId,
                ServiceName = type != null
                   ? type.FullName
                   : "",
                MethodName = method.Name,
                Parameters = ConvertArgumentsToJson(arguments),
                ExecutionTime = DateTime.Now
            };

            try
            {
                if (String.IsNullOrEmpty(auditInfo.ClientIpAddress))
                {
                    auditInfo.ClientIpAddress = _clientInfoProvider.ClientIpAddress;
                }

                if (String.IsNullOrEmpty(auditInfo.BrowserInfo))
                {
                    auditInfo.BrowserInfo = _clientInfoProvider.BrowserInfo;
                }

                if (String.IsNullOrEmpty(auditInfo.ClientName))
                {
                    auditInfo.ClientName = _clientInfoProvider.ComputerName;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.ToString(), ex);
            }

            return auditInfo;
        }

        public async Task SaveAsync(AuditInfo auditInfo)
        {
            using (_logger.BeginScope(new Dictionary<string, object>
            {
                ["userId"] = auditInfo.UserId,
                ["serviceName"] = auditInfo.ServiceName,
                ["methodName"] = auditInfo.MethodName,
                ["parameters"] = auditInfo.Parameters,
                ["returnValue"] = auditInfo.ReturnValue,
                ["executionTime"] = auditInfo.ExecutionTime,
                ["executionDuration"] = auditInfo.ExecutionDuration,
                ["clientIpAddress"] = auditInfo.ClientIpAddress,
                ["clientName"] = auditInfo.ClientName,
                ["browserInfo"] = auditInfo.BrowserInfo,
                ["customData"] = auditInfo.CustomData
            }))
            {
                _logger.LogInformation(auditInfo.GetLoginfo());
            }
            using (var uow = _unitOfWork.Begin(TransactionScopeOption.Suppress))
            {
                //await _auditLogRepository.InsertAsync(AuditLog.CreateFromAuditInfo(auditInfo));

                await uow.CommitAsync();
            }
        }

        private string ConvertArgumentsToJson(IDictionary<string, object> arguments)
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
                    if (argument.Value != null && _configuration.IgnoredTypes.Any(t => t.IsInstanceOfType(argument.Value)))
                    {
                        dictionary[argument.Key] = null;
                    }
                    else
                    {
                        dictionary[argument.Key] = argument.Value;
                    }
                }

                return _auditSerializer.Serialize(dictionary);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.ToString(), ex);
                return "{}";
            }
        }
    }
}