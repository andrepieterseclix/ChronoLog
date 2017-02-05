using CLog.UI.Common.Business;
using CLog.UI.Models.Timesheets;
using System;

namespace CLog.UI.CaptureTime.Managers
{
    /// <summary>
    /// Represents the client-side Timesheet Manager contract.
    /// </summary>
    public interface ITimesheetManager
    {
        /// <summary>
        /// Gets the captured time.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns>
        /// The business result containing the captured time items.
        /// </returns>
        BusinessResult<CaptureTimeDay[]> GetCapturedTime(DateTime fromDate, DateTime toDate);

        /// <summary>
        /// Saves the captured time.
        /// </summary>
        /// <param name="models">The models.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns>
        /// The business result.
        /// </returns>
        BusinessResult SaveCapturedTime(CaptureTimeDay[] models, string userName);
    }
}
