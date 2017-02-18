using CLog.Common.Logging;
using CLog.Framework.ServiceClients;
using CLog.ServiceClients.Contracts.Timesheets;
using CLog.Services.Contracts.Timesheets;
using CLog.Services.Models.Timesheets.DataTransfer;
using CLog.UI.CaptureTime.Extensions;
using CLog.UI.Common.Business;
using CLog.UI.Models.Timesheets;
using System;

namespace CLog.UI.CaptureTime.Managers
{
    /// <summary>
    /// Represents the client-side Timesheet Manager.
    /// </summary>
    public sealed class TimesheetManager : UIBusinessManager, ITimesheetManager
    {
        #region Fields

        private readonly ITimesheetClientFactory _timesheetClientFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TimesheetManager" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="timesheetClientFactory">The timesheet client factory.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public TimesheetManager(ILogger logger, ITimesheetClientFactory timesheetClientFactory)
            : base(logger)
        {
            if (timesheetClientFactory == null)
                throw new ArgumentNullException(nameof(timesheetClientFactory));

            _timesheetClientFactory = timesheetClientFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the captured time.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns>
        /// The captured time items.
        /// </returns>
        public UIBusinessResult<CaptureTimeDay[]> GetCapturedTime(DateTime fromDate, DateTime toDate)
        {
            return Execute<CaptureTimeDay[]>(result =>
            {
                GetCapturedTimeResponse response = null;
                GetCapturedTimeRequest request = new GetCapturedTimeRequest(fromDate, toDate);

                using (IServiceClient<ITimesheetService> client = _timesheetClientFactory.Create())
                {
                    response = GetServiceResponse(client.Proxy.GetCapturedTime, request);
                }

                // Map Errors
                result.AddMessages(response);

                // Map Response
                result.Result = response.CapturedTimeItems.Map();
            });
        }

        /// <summary>
        /// Saves the captured time.
        /// </summary>
        /// <param name="models">The models.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns>The business result.</returns>
        public UIBusinessResult SaveCapturedTime(CaptureTimeDay[] models, string userName)
        {
            return Execute(result =>
            {
                SaveCapturedTimeRequest request = new SaveCapturedTimeRequest(models.Map(userName));
                SaveCapturedTimeResponse response;

                using (IServiceClient<ITimesheetService> client = _timesheetClientFactory.Create())
                {
                    response = GetServiceResponse(client.Proxy.SaveCapturedTime, request);
                }

                // Map Errors
                result.AddMessages(response);
            });
        }

        #endregion
    }
}
