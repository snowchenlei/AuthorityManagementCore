using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Anc.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Anc.Auditing
{
    public class SimpleLogAuditingStore : IAuditingStore, ISingletonDependency
    {
        public ILogger<SimpleLogAuditingStore> Logger { get; set; }

        public SimpleLogAuditingStore()
        {
            Logger = NullLogger<SimpleLogAuditingStore>.Instance;
        }

        public void Save(AuditLogInfo auditInfo)
        {
            Logger.LogInformation(auditInfo.ToString());
        }

        public Task SaveAsync(AuditLogInfo auditInfo)
        {
            Save(auditInfo);
            return Task.FromResult(0);
        }
    }
}