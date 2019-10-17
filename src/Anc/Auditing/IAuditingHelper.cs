using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Anc.Auditing
{
    public interface IAuditingHelper

    {
        bool ShouldSaveAudit(MethodInfo methodInfo, bool defaultValue = false);

        AuditInfo CreateAuditInfo(Type type, MethodInfo method, IDictionary<string, object> arguments);

        Task SaveAsync(AuditInfo auditInfo);
    }
}