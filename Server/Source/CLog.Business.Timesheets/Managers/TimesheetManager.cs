using CLog.Business.Contracts.Timesheets;
using CLog.Business.Timesheets.Messages;
using CLog.Common.Logging;
using CLog.DataAccess.Contracts.Repositories.Access;
using CLog.DataAccess.Contracts.Repositories.Timesheets;
using CLog.Framework.Business.Exceptions;
using CLog.Framework.Business.Managers;
using CLog.Framework.Business.Models.Results;
using CLog.Models.Access;
using CLog.Models.Timesheets;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Permissions;

namespace CLog.Business.Timesheets.Managers
{
    /// <summary>
    /// Represents the Timesheet business manager.
    /// </summary>
    /// <seealso cref="CLog.Framework.Business.Managers.BusinessManager" />
    /// <seealso cref="CLog.Business.Contracts.Timesheets.ITimesheetManager" />
    public sealed class TimesheetManager : BusinessManager, ITimesheetManager
    {
        #region Fields

        public const int CAPTURED_TIME_QUERY_MAX_DAY_SPAN = 90;

        private readonly ICapturedTimeRepository _captureTimeRepository;

        private readonly IUserRepository _userRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TimesheetManager" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="captureTimeRepository">The capture time repository.</param>
        /// <param name="userRepository">The user repository.</param>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        public TimesheetManager(ILogger logger, ICapturedTimeRepository captureTimeRepository, IUserRepository userRepository)
            : base(logger)
        {
            if (captureTimeRepository == null)
                throw new ArgumentNullException(nameof(captureTimeRepository));
            if (userRepository == null)
                throw new ArgumentNullException(nameof(userRepository));

            _captureTimeRepository = captureTimeRepository;
            _userRepository = userRepository;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the captured time.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns>
        /// The captured time items.
        /// </returns>
        [PrincipalPermission(SecurityAction.Demand)]
        public BusinessResult<CapturedTime[]> GetCapturedTime(DateTime fromDate, DateTime toDate)
        {
            BusinessResult<CapturedTime[]> result = new BusinessResult<CapturedTime[]>();

            Execute(() =>
            {
                IDictionary<DateTime, CapturedTime> items;
                GetCapturedTimeInternal(fromDate, toDate, result, out items);

                if (!result.HasErrors)
                    result.Result = items?.Values.ToArray();
            });

            return result;
        }

        /// <summary>
        /// Saves the captured time.
        /// </summary>
        /// <param name="capturedTimeItems">The captured time items.</param>
        /// <returns>
        /// The business result.
        /// </returns>
        [PrincipalPermission(SecurityAction.Demand)]
        public BusinessResult SaveCapturedTime(CapturedTimeDetail[] capturedTimeItems)
        {
            BusinessResult result = new BusinessResult();

            Execute(() =>
            {
                // Validate
                if (capturedTimeItems == null || capturedTimeItems.Length < 1 || capturedTimeItems.Any(x => x == null))
                {
                    result.Errors.Add(ErrorMessages.CapturedTimeItemsNotValid());
                    return;
                }

                if (!capturedTimeItems.All(x => x.UserName == UserIdentity.Name))
                {
                    result.Errors.Add(ErrorMessages.InvalidUserRequest());
                    return;
                }

                // Check the range of dates
                List<CapturedTimeDetail> capturedTimeDetailsList = capturedTimeItems.OrderBy(x => x.Date).ToList();
                capturedTimeDetailsList.ForEach(x => x.Date = x.Date.Date);
                DateTime fromDate = capturedTimeItems[0].Date;
                DateTime toDate = capturedTimeItems[capturedTimeItems.Length - 1].Date;

                // Fetch models
                IDictionary<DateTime, CapturedTime> items;
                GetCapturedTimeInternal(fromDate, toDate, result, out items);

                if (result.HasErrors)
                    return;

                // Changes to the data context need to be made within a transaction scope, to succeed or fail as a whole.
                _captureTimeRepository.ManualCommit = true;

                // Map data to models to be updated
                foreach (CapturedTimeDetail detail in capturedTimeItems)
                {
                    CapturedTime updateItem = null;
                    if (!items.TryGetValue(detail.Date, out updateItem))
                    {
                        LoggerHelper.Error(Logger, "Dictionary doesn't contain a Captured Time for user Id '{0}' on date '{1}'.", UserIdentity.UserId, detail.Date);
                        continue;
                    }

                    // Apply business rules
                    if (detail.HoursWorked < 1 && updateItem.HoursWorked < 1)
                        continue;

                    if (!updateItem.IsEnabled)
                    {
                        ErrorMessage CaptureDateNotEnabledMessage = ErrorMessages.CaptureDateNotEnabled();
                        CaptureDateNotEnabledMessage.AdditionalInfo = string.Format(CultureInfo.CurrentCulture, "The date '{0}' is disabled for capturing time.", updateItem.Date);
                        result.Errors.Add(CaptureDateNotEnabledMessage);
                        continue;
                    }

                    // Update data
                    updateItem.HoursWorked = detail.HoursWorked;
                    _captureTimeRepository.Save(updateItem);

                    LoggerHelper.Info(Logger, "User '{0}' is saving {1} hours worked on {2}.  Transaction '{3}'.", UserIdentity.Name, updateItem.HoursWorked, updateItem.Date, UserIdentity.ScopeId);
                }

                if (result.HasErrors)
                    return;

                LoggerHelper.Info(Logger, "Committing Transaction '{0}'.", UserIdentity.ScopeId);
                _captureTimeRepository.SaveChanges();
                LoggerHelper.Info(Logger, "Committed Transaction '{0}'.", UserIdentity.ScopeId);
            });

            return result;
        }

        #endregion

        #region Helper Methods

        private void GetCapturedTimeInternal(DateTime fromDate, DateTime toDate, BusinessResult result, out IDictionary<DateTime, CapturedTime> capturedTimeItems, [CallerMemberName]string caller = null)
        {
            LoggerHelper.Info(Logger, "Getting captured time for caller '{0}'.", caller);

            if (result == null)
                throw new ArgumentNullException(nameof(result));

            capturedTimeItems = null;
            fromDate = fromDate.Date;
            toDate = toDate.Date;

            // Validate
            if (fromDate > toDate)
            {
                result.Errors.Add(ErrorMessages.InvalidFromAndToDate());
                return;
            }

            int days = toDate.Subtract(fromDate).Days + 1;

            if (days > CAPTURED_TIME_QUERY_MAX_DAY_SPAN)
            {
                result.Errors.Add(ErrorMessages.QueryMaxDaySpan());
                return;
            }

            LoggerHelper.Info(Logger, "User '{0}' requesting captured time for {1} days.", UserIdentity.Name, days);

            User user = _userRepository.Get(x => x.Id == UserIdentity.UserId);

            if (user == null)
            {
                LoggerHelper.Error(Logger, "The user with Id '{1}' and UserName '{0}' was not found after being authenticated!", UserIdentity.UserId, UserIdentity.Name);
                throw new BusinessException("User not found!");
            }

            // Obtain data
            CapturedTime[] items = GetUserCapturedTime(user, fromDate, toDate, result);
            if (result.HasErrors)
                return;

            IDictionary<DateTime, DateState> dateStatesDictionary = GetDateStates(fromDate, toDate, result);
            if (result.HasErrors)
                return;

            CheckDateStatesMaintenance(dateStatesDictionary.Values, fromDate, toDate);

            // Stitch data
            SortedDictionary<DateTime, CapturedTime> capturedTimeDictionary =
                new SortedDictionary<DateTime, CapturedTime>(items.ToDictionary(x => x.Date));

            for (DateTime date = fromDate; date <= toDate; date = date.AddDays(1))
            {
                CapturedTime item = null;
                if (!capturedTimeDictionary.TryGetValue(date, out item))
                {
                    item = CapturedTime.New(user, date, 0);
                    capturedTimeDictionary.Add(date, item);
                }

                // Set properties according to business rules
                DateState dateState = null;
                dateStatesDictionary.TryGetValue(date, out dateState);
                item.IsLocked = dateState?.IsLocked ?? false;
                item.IsPublicHoliday = dateState?.IsPublicHoliday ?? false;
                item.IsEnabled = !item.IsLocked && item.Date <= DateTime.Now.Date;
            }

            capturedTimeItems = capturedTimeDictionary;
        }

        private void CheckDateStatesMaintenance(ICollection<DateState> dateStates, DateTime fromDate, DateTime toDate)
        {
            // If the query is for a time period in the past, and there are no date state records, or none of them are locked, log a warning.
            if ((dateStates.Count < 1 || dateStates.All(x => !x.IsLocked)) && DateTime.Now.Date > toDate)
            {
                LoggerHelper.Warning(Logger, "Administration of Date States between '{0}' and '{1}' may be required!", fromDate, toDate);
            }
        }

        private CapturedTime[] GetUserCapturedTime(User user, DateTime fromDate, DateTime toDate, BusinessResult result)
        {
            CapturedTime[] items = user
                .CapturedTimeItems
                .Where(x => x.Date >= fromDate && x.Date <= toDate)
                .OrderBy(x => x.Date)
                .ToArray();

            if (items.Select(x => x.Date.Date).Distinct().Count() < items.Length)
            {
                LoggerHelper.Error(Logger, "User with Id '{0}' has duplicate Captured Time items between '{1}' and '{2}'!", user.Id, fromDate, toDate);
                result.Errors.Add(ErrorMessages.CapturedTimeDuplicateDates());
            }

            return items;
        }

        private IDictionary<DateTime, DateState> GetDateStates(DateTime fromDate, DateTime toDate, BusinessResult result)
        {
            DateState[] dateStates = _captureTimeRepository
                .GetAllDateStates(fromDate, toDate)
                .ToArray();

            if (dateStates.Select(x => x.Date.Date).Distinct().Count() < dateStates.Length)
            {
                LoggerHelper.Error(Logger, "Duplicate date found in the date states between '{0}' and '{1}'.", fromDate, toDate);
                ErrorMessage message = ErrorMessages.DuplicateDateState();
                message.AdditionalInfo = string.Format(CultureInfo.CurrentCulture, "Between {0} and {1}.", fromDate, toDate);
                result.Errors.Add(message);

                return null;
            }

            return dateStates
                .ToDictionary(x => x.Date);
        }

        #endregion

        #region IDisposable Implementation

        /// <summary>
        /// Keep this private, and create and maintain one for every derived class.
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_userRepository != null)
                        _userRepository.Dispose();

                    if (_captureTimeRepository != null)
                        _captureTimeRepository.Dispose();
                }

                _disposed = true;
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
