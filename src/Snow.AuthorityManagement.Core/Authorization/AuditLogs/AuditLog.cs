using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Anc.Auditing;
using Anc.Domain.Entities;
using Anc.Extensions;
using Anc.Runtime.Validation;
using Anc.UI;

namespace Snow.AuthorityManagement.Core.Authorization.AuditLogs
{
    public class AuditLog : Entity<Guid>
    {
        /// <summary>
        /// Maximum length of <see cref="ServiceName"/> property.
        /// </summary>
        public static int MaxServiceNameLength = 256;

        /// <summary>
        /// Maximum length of <see cref="MethodName"/> property.
        /// </summary>
        public static int MaxMethodNameLength = 256;

        /// <summary>
        /// Maximum length of <see cref="Parameters"/> property.
        /// </summary>
        public static int MaxParametersLength = 1024;

        /// <summary>
        /// Maximum length of <see cref="ReturnValue"/> property.
        /// </summary>
        public static int MaxReturnValueLength = 1024;

        /// <summary>
        /// Maximum length of <see cref="ClientIpAddress"/> property.
        /// </summary>
        public static int MaxClientIpAddressLength = 64;

        /// <summary>
        /// Maximum length of <see cref="ClientName"/> property.
        /// </summary>
        public static int MaxClientNameLength = 128;

        /// <summary>
        /// Maximum length of <see cref="BrowserInfo"/> property.
        /// </summary>
        public static int MaxBrowserInfoLength = 512;

        /// <summary>
        /// Maximum length of <see cref="Exception"/> property.
        /// </summary>
        public static int MaxExceptionLength = 2000;

        /// <summary>
        /// Maximum length of <see cref="CustomData"/> property.
        /// </summary>
        public static int MaxCustomDataLength = 2000;

        public virtual long? UserId
        {
            get;
            set;
        }

        public virtual string ServiceName
        {
            get;
            set;
        }

        public virtual string MethodName
        {
            get;
            set;
        }

        public virtual string Parameters
        {
            get;
            set;
        }

        public virtual string ReturnValue
        {
            get;
            set;
        }

        public virtual DateTime ExecutionTime
        {
            get;
            set;
        }

        public virtual int ExecutionDuration
        {
            get;
            set;
        }

        public virtual string ClientIpAddress
        {
            get;
            set;
        }

        public virtual string ClientName
        {
            get;
            set;
        }

        public virtual string BrowserInfo
        {
            get;
            set;
        }

        public virtual string Exception
        {
            get;
            set;
        }

        public virtual string CustomData
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a new CreateFromAuditInfo from given <see cref="auditInfo"/>.
        /// </summary>
        /// <param name="auditInfo">Source <see cref="AuditInfo"/> object</param>
        /// <returns>The <see cref="AuditLog"/> object that is created using <see cref="auditInfo"/></returns>
        public static AuditLog CreateFromAuditInfo(AuditInfo auditInfo)
        {
            var exceptionMessage = GetAncClearException(auditInfo.Exception);
            return new AuditLog
            {
                UserId = auditInfo.UserId,
                ServiceName = auditInfo.ServiceName.TruncateWithPostfix(MaxServiceNameLength),
                MethodName = auditInfo.MethodName.TruncateWithPostfix(MaxMethodNameLength),
                Parameters = auditInfo.Parameters.TruncateWithPostfix(MaxParametersLength),
                ReturnValue = auditInfo.ReturnValue.TruncateWithPostfix(MaxReturnValueLength),
                ExecutionTime = auditInfo.ExecutionTime,
                ExecutionDuration = auditInfo.ExecutionDuration,
                ClientIpAddress = auditInfo.ClientIpAddress.TruncateWithPostfix(MaxClientIpAddressLength),
                ClientName = auditInfo.ClientName.TruncateWithPostfix(MaxClientNameLength),
                BrowserInfo = auditInfo.BrowserInfo.TruncateWithPostfix(MaxBrowserInfoLength),
                Exception = exceptionMessage.TruncateWithPostfix(MaxExceptionLength),
                CustomData = auditInfo.CustomData.TruncateWithPostfix(MaxCustomDataLength)
            };
        }

        /// <summary>
        /// Make audit exceptions more explicit.
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static string GetAncClearException(System.Exception exception)
        {
            var clearMessage = "";
            switch (exception)
            {
                case null:
                    return null;

                case AncValidationException abpValidationException:
                    clearMessage = "There are " + abpValidationException.ValidationErrors.Count + " validation errors:";
                    foreach (var validationResult in abpValidationException.ValidationErrors)
                    {
                        var memberNames = "";
                        if (validationResult.MemberNames != null && validationResult.MemberNames.Any())
                        {
                            memberNames = " (" + string.Join(", ", validationResult.MemberNames) + ")";
                        }

                        clearMessage += "\r\n" + validationResult.ErrorMessage + memberNames;
                    }
                    break;
            }

            return exception + (clearMessage.IsNullOrWhiteSpace() ? "" : "\r\n\r\n" + clearMessage);
        }
    }
}