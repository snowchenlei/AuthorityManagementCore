using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Anc.Auditing
{
    public interface IAuditingStore
    {
        void Save(AuditLogInfo auditInfo);

        Task SaveAsync(AuditLogInfo auditInfo);
    }
}