using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Anc.DynamicProxy
{
    public interface IAncInterceptor
    {
        void Intercept(IAncMethodInvocation invocation);

        Task InterceptAsync(IAncMethodInvocation invocation);
    }
}