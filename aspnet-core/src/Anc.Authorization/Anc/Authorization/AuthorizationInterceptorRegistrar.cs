﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Anc.DependencyInjection;
using Microsoft.AspNetCore.Authorization;

namespace Anc.Authorization.Anc.Authorization
{
    public static class AuthorizationInterceptorRegistrar
    {
        public static void RegisterIfNeeded(IOnServiceRegistredContext context)
        {
            if (ShouldIntercept(context.ImplementationType))
            {
                //context.Interceptors.TryAdd<AuthorizationInterceptor>();
            }
        }

        private static bool ShouldIntercept(Type type)
        {
            return type.IsDefined(typeof(AuthorizeAttribute), true) ||
                   AnyMethodHasAuthorizeAttribute(type);
        }

        private static bool AnyMethodHasAuthorizeAttribute(Type implementationType)
        {
            return implementationType
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Any(HasAuthorizeAttribute);
        }

        private static bool HasAuthorizeAttribute(MemberInfo methodInfo)
        {
            return methodInfo.IsDefined(typeof(AuthorizeAttribute), true);
        }
    }
}