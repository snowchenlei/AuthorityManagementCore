using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Snow.AuthorityManagement.Core.Exception
{
    public class AncInitializationException : System.Exception
    {
        /// Constructor.
        /// </summary>
        public AncInitializationException()
        {
        }

        /// <summary>
        /// Constructor for serializing.
        /// </summary>
        public AncInitializationException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Exception message</param>
        public AncInitializationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public AncInitializationException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }
    }
}