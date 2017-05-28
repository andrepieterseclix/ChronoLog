using CLog.Framework.Business.Models.Results;
using CLog.Framework.Models;

namespace CLog.Framework.Business.Messages
{
    /// <summary>
    /// Represents the container for global Error Messages across all business modules.
    /// </summary>
    public static class ErrorMessages
    {
        #region Constants

        private const string UNHANDLED_BUSINESS_EXCEPTION_CODE = "GE0001";

        private const string UNHANDLED_BUSINESS_EXCEPTION_MESSAGE = "An error was encountered while servicing your request.";

        private const string SESSION_EXPIRED_CODE = "GE0001";

        private const string SESSION_EXPIRED_MESSAGE = "The session has expired, please log in again.";

        #endregion

        #region Messages

        /// <summary>
        /// The unhandled business exception error message.
        /// </summary>
        public static ErrorMessage UnhandledBusinessException()
        {
            return new ErrorMessage(ErrorCategory.General, UNHANDLED_BUSINESS_EXCEPTION_CODE, UNHANDLED_BUSINESS_EXCEPTION_MESSAGE);
        }

        /// <summary>
        /// The session expired error message.
        /// </summary>
        public static ErrorMessage SessionExpired()
        {
            return new ErrorMessage(ErrorCategory.General, SESSION_EXPIRED_CODE, SESSION_EXPIRED_MESSAGE);
        }

        #endregion
    }
}
