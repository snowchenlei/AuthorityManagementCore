using System;
using System.Collections.Generic;
using System.Text;
using Anc.Collections;
using Anc.DynamicProxy;

namespace Anc.DependencyInjection
{
    public interface IOnServiceRegistredContext
    {
        ITypeList<IAncInterceptor> Interceptors { get; }

        Type ImplementationType { get; }
    }
}