using CLog.Framework.Business.Models.Results;
using CLog.Framework.Models;

namespace CLog.Business.Access.Messages
{
    /// <summary>
    /// Represents the container for Error Messages for the Access business module.
    /// </summary>
    public static class ErrorMessages
    {
        #region Constants

        private const string INCORRECT_USER_OR_PASSWORD_CODE = "AM0001";

        private const string INCORRECT_USER_OR_PASSWORD_MESSAGE = "Incorrect user name or password!";

        private const string USER_NOT_APPROVED_CODE = "AM0002";

        private const string USER_NOT_APPROVED_MESSAGE = "The specified user has not been approved, please contact the system administrator.";

        private const string USER_SUSPENDED_CODE = "AM0003";

        private const string USER_SUSPENDED_MESSAGE = "The user has been suspended, please contact the system administrator.";

        private const string USER_INCONSISTENT_STATE_CODE = "AM0004";

        private const string USER_INCONSISTENT_STATE_MESSAGE = "The user is in an invalid state, please contact the system administrator.";

        private const string SESSION_NOT_FOUND_CODE = "AM0005";

        private const string SESSION_NOT_FOUND_MESSAGE = "The specified session could not be found.";

        private const string INVALID_SESSION_CODE = "AM0006";

        private const string INVALID_SESSION_MESSAGE = "The specified session is invalid, please log in again.";

        private const string INCORRECT_OLD_PASSWORD_CODE = "AM0007";

        private const string INCORRECT_OLD_PASSWORD_MESSAGE = "The specified old password is incorrect.";

        private const string INVALID_PASSWORD_CODE = "AM0008";

        private const string INVALID_PASSWORD_MESSAGE = "The specified new password is invalid.";

        #endregion

        #region Messages

        /// <summary>
        /// The user not found error message.
        /// </summary>
        public static ErrorMessage UserNotFound()
        {
            // Cannot be "NotFound" category!
            return new ErrorMessage(ErrorCategory.InvalidRequest, INCORRECT_USER_OR_PASSWORD_CODE, INCORRECT_USER_OR_PASSWORD_MESSAGE);
        }

        /// <summary>
        /// The incorrect password error message.
        /// </summary>
        public static ErrorMessage IncorrectPassword()
        {
            return new ErrorMessage(ErrorCategory.InvalidRequest, INCORRECT_USER_OR_PASSWORD_CODE, INCORRECT_USER_OR_PASSWORD_MESSAGE);
        }

        /// <summary>
        /// Gets the user not approved error message.
        /// </summary>
        public static ErrorMessage UserNotApproved()
        {
            return new ErrorMessage(ErrorCategory.InvalidRequest, USER_NOT_APPROVED_CODE, USER_NOT_APPROVED_MESSAGE);
        }

        /// <summary>
        /// The user suspended error message.
        /// </summary>
        public static ErrorMessage UserSuspended()
        {
            return new ErrorMessage(ErrorCategory.InvalidRequest, USER_SUSPENDED_CODE, USER_SUSPENDED_MESSAGE);
        }

        /// <summary>
        /// Gets the user inconsistent state error message.
        /// </summary>
        public static ErrorMessage UserInconsistentState()
        {
            return new ErrorMessage(ErrorCategory.InvalidRequest, USER_INCONSISTENT_STATE_CODE, USER_INCONSISTENT_STATE_MESSAGE);
        }

        /// <summary>
        /// Gets the session not found error message.
        /// </summary>
        public static ErrorMessage SessionNotFound()
        {
            return new ErrorMessage(ErrorCategory.ResourceNotFound, SESSION_NOT_FOUND_CODE, SESSION_NOT_FOUND_MESSAGE);
        }

        /// <summary>
        /// Gets the invalid session error message.
        /// </summary>
        public static ErrorMessage InvalidSession()
        {
            return new ErrorMessage(ErrorCategory.InvalidRequest, INVALID_SESSION_CODE, INVALID_SESSION_MESSAGE);
        }

        /// <summary>
        /// The incorrect old password error message.
        /// </summary>
        public static ErrorMessage IncorrectOldPassword()
        {
            return new ErrorMessage(ErrorCategory.InvalidRequest, INCORRECT_OLD_PASSWORD_CODE, INCORRECT_OLD_PASSWORD_MESSAGE);
        }

        /// <summary>
        /// The invalid password error message.
        /// </summary>
        public static ErrorMessage InvalidPassword()
        {
            return new ErrorMessage(ErrorCategory.InvalidRequest, INVALID_PASSWORD_CODE, INVALID_PASSWORD_MESSAGE);
        }

        #endregion
    }
}
