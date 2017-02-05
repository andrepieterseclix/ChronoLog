using CLog.Business.Timesheets.Managers;
using CLog.Framework.Business.Models.Results;

namespace CLog.Business.Timesheets.Messages
{
    /// <summary>
    /// Represents the container for Error Messages for the Timesheets business module.
    /// </summary>
    public static class ErrorMessages
    {
        #region Constants

        private const string INVALID_FROM_AND_TO_DATE_CODE = "TS0001";

        private const string INVALID_FROM_AND_TO_DATE_MESSAGE = "The from-date must be earlier than the to-date.";

        private const string QUERY_MAX_DAY_SPAN_CODE = "TS0002";

        private const string QUERY_MAX_DAY_SPAN_MESSAGE = "The number of days requested is too large.";

        private const string CAPTURED_TIME_DUPLICATE_DATES_CODE = "TS0003";

        private static string CAPTURED_TIME_DUPLICATE_DATES_MESSAGE = "There is a data inconsistency on the server, duplicate date items were found.";

        private const string CAPTURED_TIME_ITEMS_NOT_SPECIFIED_CODE = "TS0004";

        private static string CAPTURED_TIME_ITEMS_NOT_SPECIFIED_MESSAGE = "The request parameters are invalid.";

        private const string INVALID_USER_REQUEST_CODE = "TS0005";

        private static string INVALID_USER_REQUEST_MESSAGE = "The specified user may not update another user.";

        private const string CAPTURE_DATE_NOT_ENABLED_CODE = "TS0006";

        private static string CAPTURE_DATE_NOT_ENABLED_MESSAGE = "Hours can not be captured for a date that is disabled.";

        private const string DUPLICATE_DATE_STATE_CODE = "TS0007";

        private static string DUPLICATE_DATE_STATE_MESSAGE = "Duplicate date state found.";

        #endregion

        #region Properties

        /// <summary>
        /// The invalid from and to date error message.
        /// </summary>
        public static ErrorMessage InvalidFromAndToDate()
        {
            return new ErrorMessage(INVALID_FROM_AND_TO_DATE_CODE, INVALID_FROM_AND_TO_DATE_MESSAGE);
        }

        /// <summary>
        /// The query maximum day span error message.
        /// </summary>
        public static ErrorMessage QueryMaxDaySpan()
        {
            return new ErrorMessage(QUERY_MAX_DAY_SPAN_CODE, QUERY_MAX_DAY_SPAN_MESSAGE, string.Format("The number of days between the from-date and the to-date must be less than {0} days.", TimesheetManager.CAPTURED_TIME_QUERY_MAX_DAY_SPAN));
        }

        /// <summary>
        /// The captured time duplicate dates error message.
        /// </summary>
        public static ErrorMessage CapturedTimeDuplicateDates()
        {
            return new ErrorMessage(CAPTURED_TIME_DUPLICATE_DATES_CODE, CAPTURED_TIME_DUPLICATE_DATES_MESSAGE);
        }

        /// <summary>
        /// The captured time items not specified error message.
        /// </summary>
        public static ErrorMessage CapturedTimeItemsNotValid()
        {
            return new ErrorMessage(CAPTURED_TIME_ITEMS_NOT_SPECIFIED_CODE, CAPTURED_TIME_ITEMS_NOT_SPECIFIED_MESSAGE);
        }

        /// <summary>
        /// The invalid user request error message.
        /// </summary>
        public static ErrorMessage InvalidUserRequest()
        {
            return new ErrorMessage(INVALID_USER_REQUEST_CODE, INVALID_USER_REQUEST_MESSAGE);
        }

        /// <summary>
        /// The capture date not enabled error message.
        /// </summary>
        public static ErrorMessage CaptureDateNotEnabled()
        {
            return new ErrorMessage(CAPTURE_DATE_NOT_ENABLED_CODE, CAPTURE_DATE_NOT_ENABLED_MESSAGE);
        }

        /// <summary>
        /// Gets the state of the duplicate date error message.
        /// </summary>
        public static ErrorMessage DuplicateDateState()
        {
            return new ErrorMessage(DUPLICATE_DATE_STATE_CODE, DUPLICATE_DATE_STATE_MESSAGE);
        }

        #endregion
    }
}
