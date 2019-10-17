using System;
using System.Collections.Generic;
using System.Text;

namespace Anc.Auditing
{
    public class AuditingConfiguration : IAuditingConfiguration
    {
        public bool IsEnabled { get; set; }

        public bool IsEnabledForAnonymousUsers { get; set; }

        public List<Type> IgnoredTypes { get; }

        public bool SaveReturnValues { get; set; }

        public AuditingConfiguration()
        {
            IsEnabled = true;
            IgnoredTypes = new List<Type>();
            SaveReturnValues = false;
        }
    }
}