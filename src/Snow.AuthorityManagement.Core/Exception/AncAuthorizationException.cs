using System;
using System.Collections.Generic;
using System.Text;

namespace Snow.AuthorityManagement.Core.Exception
{
    public class AncAuthorizationException : System.Exception
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public AncAuthorizationException()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Exception message</param>
        public AncAuthorizationException(string message)
            : base(message)
        {
        }
    }
}