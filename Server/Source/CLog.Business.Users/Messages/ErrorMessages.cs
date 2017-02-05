using CLog.Framework.Business.Models.Results;

namespace CLog.Business.Users.Messages
{
    /// <summary>
    /// Represents the container for Error Messages for the Users business module.
    /// </summary>
    public static class ErrorMessages
    {
        #region Constants

        private const string INVALID_REQUEST_ARGUMENTS_CODE = "UM0001";

        private const string INVALID_REQUEST_ARGUMENTS_MESSAGE = "Please specify the user Details or the user Password to update.";

        private const string USER_NAME_NOT_SPECIFIED_CODE = "UM0002";

        private const string USER_NAME_NOT_SPECIFIED_MESSAGE = "The user name has not been specified.";

        private const string USER_NOT_FOUND_CODE = "UM0003";

        private const string USER_NOT_FOUND_MESSAGE = "The user does not exist.";

        private const string INVALID_EMAIL_ADDRESS_CODE = "UM0004";

        private const string INVALID_EMAIL_ADDRESS_MESSAGE = "The specified e-mail address is invalid.";

        private const string INVALID_NAME_OR_SURNAME_CODE = "UM0005";

        private const string INVALID_NAME_OR_SURNAME_MESSAGE = "The specified Name or Surname is invalid.";

        private const string CANNOT_UPDATE_TWO_USERS_CODE = "UM0006";

        private const string CANNOT_UPDATE_TWO_USERS_MESSAGE = "Updating two users simultaneously is not allowed.";

        #endregion

        #region Messages

        /// <summary>
        /// The invalid request arguments error message.
        /// </summary>
        public static ErrorMessage InvalidRequestArguments()
        {
            return new ErrorMessage(INVALID_REQUEST_ARGUMENTS_CODE, INVALID_REQUEST_ARGUMENTS_MESSAGE); ;
        }

        /// <summary>
        /// The user name not specified error message.
        /// </summary>
        public static ErrorMessage UserNameNotSpecified()
        {
            return new ErrorMessage(USER_NAME_NOT_SPECIFIED_CODE, USER_NAME_NOT_SPECIFIED_MESSAGE);
        }

        /// <summary>
        /// The user not found error message.
        /// </summary>
        public static ErrorMessage UserNotFound()
        {
            return new ErrorMessage(USER_NOT_FOUND_CODE, USER_NOT_FOUND_MESSAGE);
        }

        /// <summary>
        /// The invalid email address error message.
        /// </summary>
        public static ErrorMessage InvalidEmailAddress()
        {
            return new ErrorMessage(INVALID_EMAIL_ADDRESS_CODE, INVALID_EMAIL_ADDRESS_MESSAGE);
        }

        /// <summary>
        /// The invalid name or surname error message.
        /// </summary>
        public static ErrorMessage InvalidNameOrSurname()
        {
            return new ErrorMessage(INVALID_NAME_OR_SURNAME_CODE, INVALID_NAME_OR_SURNAME_MESSAGE);
        }

        /// <summary>
        /// The cannot update two users error message.
        /// </summary>
        public static ErrorMessage CannotUpdateTwoUsers()
        {
            return new ErrorMessage(CANNOT_UPDATE_TWO_USERS_CODE, CANNOT_UPDATE_TWO_USERS_MESSAGE);
        }

        #endregion
    }
}
