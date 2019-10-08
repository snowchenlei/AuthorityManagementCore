namespace Anc.UI
{
    public class UserFriendlyException : System.Exception
    {
        /// <summary>
        /// Additional information about the exception.
        /// </summary>
        public string Details { get; private set; }

        /// <summary>
        /// An arbitrary error code.
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public UserFriendlyException()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Exception message</param>
        public UserFriendlyException(string message)
            : base(message)
        {
        }
    }
}