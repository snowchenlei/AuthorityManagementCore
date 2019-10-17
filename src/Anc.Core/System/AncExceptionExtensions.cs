﻿using System.Collections.Generic;
using System.Text;
using Anc.Logging;
using Microsoft.Extensions.Logging;

namespace System
{
    /// <summary>
    /// Extension methods for <see cref="Exception"/> class.
    /// </summary>
    public static class AbpExceptionExtensions
    {
        /// <summary>
        /// Try to get a log level from the given <paramref name="exception"/> if it implements the
        /// <see cref="IHasLogLevel"/> interface. Otherwise, returns the <paramref name="defaultLevel"/>.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="defaultLevel"></param>
        /// <returns></returns>
        public static LogLevel GetLogLevel(this Exception exception, LogLevel defaultLevel = LogLevel.Error)
        {
            return (exception as IHasLogLevel)?.LogLevel ?? defaultLevel;
        }
    }
}