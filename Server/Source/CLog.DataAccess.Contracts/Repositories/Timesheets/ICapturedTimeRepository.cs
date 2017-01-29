using CLog.Models.Timesheets;
using System;
using System.Collections.Generic;

namespace CLog.DataAccess.Contracts.Repositories.Timesheets
{
    /// <summary>
    /// Represents the Captured Time repository contract.
    /// </summary>
    /// <seealso cref="CLog.DataAccess.Contracts.Repositories.IRepository{CLog.Models.Timesheets.CapturedTime}" />
    public interface ICapturedTimeRepository : IRepository<CapturedTime>
    {
        /// <summary>
        /// Gets all dates.
        /// </summary>
        /// <param name="from">From date.</param>
        /// <param name="to">To date.</param>
        /// <returns>The date sates.</returns>
        IEnumerable<DateState> GetAllDateStates(DateTime from, DateTime to);
    }
}
