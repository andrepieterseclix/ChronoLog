using CLog.Framework.Business.Models.Results;

namespace CLog.Framework.Business.Messages
{
    /// <summary>
    /// Represents the container for global Error Messages across all business modules.
    /// </summary>
    public static class ErrorMessages
    {
        #region Constants

        private const string UnhandledBusinessExceptionCode = "GE0001";

        private const string UnhandledBusinessExceptionMessage = "An error was encountered while servicing your request.";

        private const string SessionExpiredCode = "GE0001";

        private const string SessionExpiredMessage = "The session has expired, please log in again.";

        #endregion

        #region Messages

        /// <summary>
        /// The unhandled business exception error message.
        /// </summary>
        public static ErrorMessage UnhandledBusinessException = new ErrorMessage(UnhandledBusinessExceptionCode, UnhandledBusinessExceptionMessage);

        /// <summary>
        /// The session expired error message.
        /// </summary>
        public static ErrorMessage SessionExpired = new ErrorMessage(SessionExpiredCode, SessionExpiredMessage);

        #endregion
    }
}
