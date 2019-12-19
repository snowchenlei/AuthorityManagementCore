using System;
using System.Collections.Generic;
using System.Text;

namespace Anc.Auditing
{
    public class AuditLogInfo
    {
        public string ApplicationName { get; set; }

        public Guid? UserId { get; set; }

        public string UserName { get; set; }

        public Guid? TenantId { get; set; }

        public string TenantName { get; set; }

        public Guid? ImpersonatorUserId { get; set; }

        public Guid? ImpersonatorTenantId { get; set; }

        public DateTime ExecutionTime { get; set; }

        public int ExecutionDuration { get; set; }

        public string ClientId { get; set; }

        public string CorrelationId { get; set; }

        public string ClientIpAddress { get; set; }

        public string ClientName { get; set; }

        public string BrowserInfo { get; set; }

        public string HttpMethod { get; set; }

        public int? HttpStatusCode { get; set; }

        public string Url { get; set; }

        public List<AuditLogActionInfo> Actions { get; set; }

        public List<Exception> Exceptions { get; }

        public Dictionary<string, object> ExtraProperties { get; }

        public List<string> Comments { get; set; }

        public AuditLogInfo()
        {
            Actions = new List<AuditLogActionInfo>();
            Exceptions = new List<Exception>();
            ExtraProperties = new Dictionary<string, object>();
            Comments = new List<string>();
        }
    }
}