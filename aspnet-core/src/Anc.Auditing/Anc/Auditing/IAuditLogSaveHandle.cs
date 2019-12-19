using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Anc.Auditing
{
    public interface IAuditLogSaveHandle : IDisposable
    {
        void Save();

        Task SaveAsync();
    }
}