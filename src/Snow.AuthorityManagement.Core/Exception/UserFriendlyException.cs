namespace Snow.AuthorityManagement.Core.Exception
{
    public class UserFriendlyException : System.Exception
    {
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