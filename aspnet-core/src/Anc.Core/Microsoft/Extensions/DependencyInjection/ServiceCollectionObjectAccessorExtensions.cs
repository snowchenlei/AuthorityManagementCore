﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anc.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionObjectAccessorExtensions
    {
        public static ObjectAccessor<T> TryAddObjectAccessor<T>(this IServiceCollection services)
        {
            if (services.Any(s => s.ServiceType == typeof(ObjectAccessor<T>)))
            {
                return services.GetSingletonInstance<ObjectAccessor<T>>();
            }

            return services.AddObjectAccessor<T>();
        }

        public static ObjectAccessor<T> AddObjectAccessor<T>(this IServiceCollection services)
        {
            return services.AddObjectAccessor(new ObjectAccessor<T>());
        }

        public static ObjectAccessor<T> AddObjectAccessor<T>(this IServiceCollection services, T obj)
        {
            return services.AddObjectAccessor(new ObjectAccessor<T>(obj));
        }

        public static ObjectAccessor<T> AddObjectAccessor<T>(this IServiceCollection services, ObjectAccessor<T> accessor)
        {
            if (services.Any(s => s.ServiceType == typeof(ObjectAccessor<T>)))
            {
                throw new Exception("An object accessor is registered before for type: " + typeof(T).AssemblyQualifiedName);
            }

            //Add to the beginning for fast retrieve
            services.Insert(0, ServiceDescriptor.Singleton(typeof(ObjectAccessor<T>), accessor));
            services.Insert(0, ServiceDescriptor.Singleton(typeof(IObjectAccessor<T>), accessor));

            return accessor;
        }
    }
}