using System;
using System.Collections.Generic;
using System.Text;

namespace Snow.AuthorityManagement.Core.Exception
{
    /// <summary>
    /// 认证异常，没有权限
    /// </summary>
    public class AncAuthenticationException : System.Exception
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public AncAuthenticationException()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Exception message</param>
        public AncAuthenticationException(string message)
            : base(message)
        {
        }
    }
}