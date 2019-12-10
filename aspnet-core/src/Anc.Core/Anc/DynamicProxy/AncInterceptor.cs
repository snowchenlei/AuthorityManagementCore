using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Anc.DynamicProxy;

namespace Anc.Core.Anc.DynamicProxy
{
    public abstract class AncInterceptor : IAncInterceptor
    {
        public abstract void Intercept(IAncMethodInvocation invocation);

        public virtual Task InterceptAsync(IAncMethodInvocation invocation)
        {
            Intercept(invocation);
            return Task.CompletedTask;
        }
    }
}