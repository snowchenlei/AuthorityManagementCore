using System;
using System.Collections.Generic;
using System.Text;

namespace Anc.Authorization
{
    [Serializable]
    public class AncAuthorizationException : Exception
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