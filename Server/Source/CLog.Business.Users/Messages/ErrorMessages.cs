using CLog.Framework.Business.Models.Results;

namespace CLog.Business.Users.Messages
{
    /// <summary>
    /// Represents the container for Error Messages for the Users business module.
    /// </summary>
    public static class ErrorMessages
    {
        #region Constants

        private const string InvalidRequestArgumentsCode = "UM0001";

        private const string InvalidRequestArgumentsMessage = "Please specify the user Details or the user Password to update.";

        private const string UserNameNotSpecifiedCode = "UM0002";

        private const string UserNameNotSpecifiedMessage = "The user name has not been specified.";

        private const string UserNotFoundCode = "UM0003";

        private const string UserNotFoundMessage = "The user does not exist.";

        private const string InvalidEmailAddressCode = "UM0004";

        private const string InvalidEmailAddressMessage = "The specified e-mail address is invalid.";

        private const string InvalidNameOrSurnameCode = "UM0005";

        private const string InvalidNameOrSurnameMessage = "The specified Name or Surname is invalid.";

        private const string CannotUpdateTwoUsersCode = "UM0006";

        private const string CannotUpdateTwoUsersMessage = "Updating two users simultaneously is not allowed.";
        
        #endregion

        #region Messages

        /// <summary>
        /// The invalid request arguments error message.
        /// </summary>
        public static ErrorMessage InvalidRequestArguments = new ErrorMessage(InvalidRequestArgumentsCode, InvalidRequestArgumentsMessage);

        /// <summary>
        /// The user name not specified error message.
        /// </summary>
        public static ErrorMessage UserNameNotSpecified = new ErrorMessage(UserNameNotSpecifiedCode, UserNameNotSpecifiedMessage);

        /// <summary>
        /// The user not found error message.
        /// </summary>
        public static ErrorMessage UserNotFound = new ErrorMessage(UserNotFoundCode, UserNotFoundMessage);

        /// <summary>
        /// The invalid email address error message.
        /// </summary>
        public static ErrorMessage InvalidEmailAddress = new ErrorMessage(InvalidEmailAddressCode, InvalidEmailAddressMessage);

        /// <summary>
        /// The invalid name or surname error message.
        /// </summary>
        public static ErrorMessage InvalidNameOrSurname = new ErrorMessage(InvalidNameOrSurnameCode, InvalidNameOrSurnameMessage);

        /// <summary>
        /// The cannot update two users error message.
        /// </summary>
        public static ErrorMessage CannotUpdateTwoUsers = new ErrorMessage(CannotUpdateTwoUsersCode, CannotUpdateTwoUsersMessage);
        
        #endregion
    }
}
