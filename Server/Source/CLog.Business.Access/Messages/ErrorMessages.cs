using CLog.Framework.Business.Models.Results;

namespace CLog.Business.Access.Messages
{
    /// <summary>
    /// Represents the container for Error Messages for the Access business module.
    /// </summary>
    public static class ErrorMessages
    {
        #region Constants

        private const string IncorrectUserOrPasswordCode = "AM0001";

        private const string IncorrectUserOrPasswordMessage = "Incorrect user name or password!";

        private const string UserNotApprovedCode = "AM0002";

        private const string UserNotApprovedMessage = "The specified user has not been approved, please contact the system administrator.";

        private const string UserSuspendedCode = "AM0003";

        private const string UserSuspendedMessage = "The user has been suspended, please contact the system administrator.";

        private const string UserInconsistentStateCode = "AM0004";

        private const string UserInconsistentStateMessage = "The user is in an invalid state, please contact the system administrator.";

        private const string SessionNotFoundCode = "AM0005";

        private const string SessionNotFoundMessage = "The specified session could not be found.";

        private const string InvalidSessionCode = "AM0006";

        private const string InvalidSessionMessage = "The specified session is invalid, please log in again.";

        private const string IncorrectOldPasswordCode = "AM0007";

        private const string IncorrectOldPasswordMessage = "The specified old password is incorrect.";

        private const string InvalidPasswordCode = "AM0008";

        private const string InvalidPasswordMessage = "The specified new password is invalid.";

        #endregion

        #region Messages

        /// <summary>
        /// The user not found error message.
        /// </summary>
        public static ErrorMessage UserNotFound = new ErrorMessage(IncorrectUserOrPasswordCode, IncorrectUserOrPasswordMessage);

        /// <summary>
        /// The incorrect password error message.
        /// </summary>
        public static ErrorMessage IncorrectPassword = new ErrorMessage(IncorrectUserOrPasswordCode, IncorrectUserOrPasswordMessage);

        /// <summary>
        /// Gets the user not approved error message.
        /// </summary>
        public static ErrorMessage UserNotApproved = new ErrorMessage(UserNotApprovedCode, UserNotApprovedMessage);

        /// <summary>
        /// The user suspended error message.
        /// </summary>
        public static ErrorMessage UserSuspended = new ErrorMessage(UserSuspendedCode, UserSuspendedMessage);

        /// <summary>
        /// Gets the user inconsistent state error message.
        /// </summary>
        public static ErrorMessage UserInconsistentState = new ErrorMessage(UserInconsistentStateCode, UserInconsistentStateMessage);

        /// <summary>
        /// Gets the session not found error message.
        /// </summary>
        public static ErrorMessage SessionNotFound = new ErrorMessage(SessionNotFoundCode, SessionNotFoundMessage);

        /// <summary>
        /// Gets the invalid session error message.
        /// </summary>
        public static ErrorMessage InvalidSession = new ErrorMessage(InvalidSessionCode, InvalidSessionMessage);

        /// <summary>
        /// The incorrect old password error message.
        /// </summary>
        public static ErrorMessage IncorrectOldPassword = new ErrorMessage(IncorrectOldPasswordCode, IncorrectOldPasswordMessage);

        /// <summary>
        /// The invalid password error message.
        /// </summary>
        public static ErrorMessage InvalidPassword = new ErrorMessage(InvalidPasswordCode, InvalidPasswordMessage);

        #endregion
    }
}
