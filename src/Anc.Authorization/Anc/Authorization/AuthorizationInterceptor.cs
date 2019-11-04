using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Anc.Core.Anc.DynamicProxy;
using Anc.DependencyInjection;
using Anc.DynamicProxy;

namespace Anc.Authorization.Anc.Authorization
{
    public class AuthorizationInterceptor : AncInterceptor, ITransientDependency
    {
        private readonly IMethodInvocationAuthorizationService _methodInvocationAuthorizationService;

        public AuthorizationInterceptor(IMethodInvocationAuthorizationService methodInvocationAuthorizationService)
        {
            _methodInvocationAuthorizationService = methodInvocationAuthorizationService;
        }

        public override void Intercept(IAncMethodInvocation invocation)
        {
            if (AncCrossCuttingConcerns.IsApplied(invocation.TargetObject, AncCrossCuttingConcerns.Authorization))
            {
                invocation.Proceed();
                return;
            }

            AsyncHelper.RunSync(() => AuthorizeAsync(invocation));
            invocation.Proceed();
        }

        public override async Task InterceptAsync(IAbpMethodInvocation invocation)
        {
            if (AbpCrossCuttingConcerns.IsApplied(invocation.TargetObject, AbpCrossCuttingConcerns.Authorization))
            {
                await invocation.ProceedAsync();
                return;
            }

            await AuthorizeAsync(invocation);
            await invocation.ProceedAsync();
        }

        protected virtual async Task AuthorizeAsync(IAbpMethodInvocation invocation)
        {
            await _methodInvocationAuthorizationService.CheckAsync(
                new MethodInvocationAuthorizationContext(
                    invocation.Method
                )
            );
        }
    }
}