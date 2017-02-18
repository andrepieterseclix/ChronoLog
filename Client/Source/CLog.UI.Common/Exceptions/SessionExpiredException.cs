using System;

namespace CLog.UI.Common.Exceptions
{
    /// <summary>
    /// Represents the exception that is thrown on the client when the server session has expired.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class SessionExpiredException : Exception
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionExpiredException"/> class.
        /// </summary>
        public SessionExpiredException()
            : base("The session has expired!")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionExpiredException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public SessionExpiredException(string message)
            : base(message)
        {
        }

        #endregion
    }
}
