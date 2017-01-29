using CLog.Business.Contracts.Timesheets;
using CLog.Common.Logging;
using CLog.Framework.Business.Models.Results;
using CLog.Framework.Services.Extensions;
using CLog.Models.Timesheets;
using CLog.Services.Common;
using CLog.Services.Contracts.Timesheets;
using CLog.Services.Models.Timesheets.DataTransfer;
using CLog.Services.Timesheets.Extensions;
using System;
using System.Security.Permissions;

namespace CLog.Services.Timesheets
{
    /// <summary>
    /// Represents the timesheet service implementation.
    /// </summary>
    /// <seealso cref="CLog.Framework.Services.ServiceBase" />
    /// <seealso cref="CLog.Services.Contracts.Timesheets.ITimesheetService" />
    public class TimesheetService : ServiceBase, ITimesheetService
    {
        #region Fields

        private readonly ITimesheetManager _timesheetManager;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TimesheetService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="timesheetManager">The captured time repository.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public TimesheetService(ILogger logger, ITimesheetManager timesheetManager)
            : base(logger)
        {
            if (timesheetManager == null)
                throw new ArgumentNullException(nameof(timesheetManager));

            _timesheetManager = timesheetManager;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the captured time.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// The captured time response.
        /// </returns>
        [PrincipalPermission(SecurityAction.Demand)]
        public GetCapturedTimeResponse GetCapturedTime(GetCapturedTimeRequest request)
        {
            return Execute<GetCapturedTimeResponse>(response =>
            {
                BusinessResult<CapturedTime[]> result = _timesheetManager.GetCapturedTime(request.FromDate, request.ToDate);

                // Map Errors
                response.AddMessages(result);

                // Map Response
                response.CapturedTimeItems = result.Result.Map();
            });
        }

        /// <summary>
        /// Saves the captured time.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// The save capture time response.
        /// </returns>
        [PrincipalPermission(SecurityAction.Demand)]
        public SaveCapturedTimeResponse SaveCapturedTime(SaveCapturedTimeRequest request)
        {
            return Execute<SaveCapturedTimeResponse>(response =>
            {
                BusinessResult result = _timesheetManager.SaveCapturedTime(request.CapturedTimeItems.Map());

                // Map Errors
                response.AddMessages(result);
            });
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
                    if (_timesheetManager != null)
                        _timesheetManager.Dispose();
                }

                _disposed = true;
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
