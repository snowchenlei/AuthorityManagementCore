﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Anc.Threading
{
    /// <summary>
    /// Provides some helper methods to work with async methods.
    /// </summary>
    public static class AsyncHelper
    {
        /// <summary>
        /// Checks if given method is an async method.
        /// </summary>
        /// <param name="method">A method to check</param>
        public static bool IsAsync(this MethodInfo method)
        {
            return (
                method.ReturnType == typeof(Task) ||
                (method.ReturnType.GetTypeInfo().IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
            );
        }

        /// <summary>
        /// Checks if given method is an async method.
        /// </summary>
        /// <param name="method">A method to check</param>
        [Obsolete("Use MethodInfo.IsAsync() extension method!")]
        public static bool IsAsyncMethod(MethodInfo method)
        {
            return method.IsAsync();
        }
    }
}