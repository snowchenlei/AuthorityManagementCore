using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Anc.Auditing
{
    public interface IAuditingHelper
    {
        bool ShouldSaveAudit(MethodInfo methodInfo, bool defaultValue = false);

        AuditLogInfo CreateAuditLogInfo();

        AuditLogActionInfo CreateAuditLogAction(
           AuditLogInfo auditLog,
           Type type,
           MethodInfo method,
           object[] arguments
       );

        AuditLogActionInfo CreateAuditLogAction(
            AuditLogInfo auditLog,
            Type type,
            MethodInfo method,
            IDictionary<string, object> arguments
        );
    }
}