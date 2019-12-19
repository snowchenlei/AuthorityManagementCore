using System;
using System.Collections.Generic;
using System.Text;

namespace Anc.Auditing
{
    public class AuditLogActionInfo
    {
        public string ServiceName { get; set; }

        public string MethodName { get; set; }

        public string Parameters { get; set; }

        public DateTime ExecutionTime { get; set; }

        public int ExecutionDuration { get; set; }

        public Dictionary<string, object> ExtraProperties { get; }

        public AuditLogActionInfo()
        {
            ExtraProperties = new Dictionary<string, object>();
        }
    }
}