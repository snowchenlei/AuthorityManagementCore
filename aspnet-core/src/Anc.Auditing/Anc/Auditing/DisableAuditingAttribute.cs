using System;
using System.Collections.Generic;
using System.Text;

namespace Anc.Auditing
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
    public class DisableAuditingAttribute : Attribute
    {
    }
}