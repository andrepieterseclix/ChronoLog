using CLog.Framework.Business.Contracts;
using CLog.Framework.Business.Models.Results;
using CLog.Models.Timesheets;
using System;

namespace CLog.Business.Contracts.Timesheets
{
    /// <summary>
    /// Represents the timesheet manager contract.
    /// </summary>
    /// <seealso cref="CLog.Framework.Business.Contracts.IBusinessManager" />
    public interface ITimesheetManager : IBusinessManager
    {
        /// <summary>
        /// Gets the captured time.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns>The business result containing the <see cref="CapturedTime"/> items.</returns>
        BusinessResult<CapturedTime[]> GetCapturedTime(DateTime fromDate, DateTime toDate);

        /// <summary>
        /// Saves the captured time.
        /// </summary>
        /// <param name="capturedTimeItems">The captured time items.</param>
        /// <returns>
        /// The business result.
        /// </returns>
        BusinessResult SaveCapturedTime(CapturedTimeDetail[] capturedTimeItems);
    }
}
