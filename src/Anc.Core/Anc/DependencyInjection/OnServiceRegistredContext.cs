using System;
using System.Collections.Generic;
using System.Text;
using Anc.Collections;
using Anc.DependencyInjection;
using Anc.DynamicProxy;
using JetBrains.Annotations;

namespace Anc.Core.Anc.DependencyInjection
{
    public class OnServiceRegistredContext : IOnServiceRegistredContext
    {
        public virtual ITypeList<IAncInterceptor> Interceptors { get; }

        public virtual Type ServiceType { get; }

        public virtual Type ImplementationType { get; }

        public OnServiceRegistredContext(Type serviceType, [NotNull] Type implementationType)
        {
            ServiceType = Check.NotNull(serviceType, nameof(serviceType));
            ImplementationType = Check.NotNull(implementationType, nameof(implementationType));

            Interceptors = new TypeList<IAncInterceptor>();
        }
    }
}