using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Anc
{
    [Serializable]
    public class AncException : Exception
    {
        public AncException()
        {
        }

        /// <summary>
        /// Creates a new <see cref="AncException"/> object.
        /// </summary>
        public AncException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
        }

        /// <summary>
        /// Creates a new <see cref="AncException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        public AncException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates a new <see cref="AncException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public AncException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}