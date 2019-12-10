using System;
using System.Collections.Generic;
using System.Text;

namespace Anc.Auditing
{
    public static class AuditingInterceptorRegistrar
    {
        public static bool ShouldAuditTypeByDefault(Type type)
        {
            if (type.IsDefined(typeof(AuditedAttribute), true))
            {
                return true;
            }

            if (type.IsDefined(typeof(DisableAuditingAttribute), true))
            {
                return false;
            }

            if (typeof(IAuditingEnabled).IsAssignableFrom(type))
            {
                return true;
            }

            return false;
        }
    }
}