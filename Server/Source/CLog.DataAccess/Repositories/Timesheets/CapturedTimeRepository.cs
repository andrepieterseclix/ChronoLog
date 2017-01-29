using CLog.DataAccess.Contracts.Repositories.Timesheets;
using CLog.Models.Timesheets;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CLog.DataAccess.Repositories.Timesheets
{
    /// <summary>
    /// Represents the Captured Time repository.
    /// </summary>
    /// <seealso cref="CLog.DataAccess.Repositories.GenericRepository{CLog.Models.Timesheets.CapturedTime}" />
    /// <seealso cref="CLog.DataAccess.Contracts.Repositories.Timesheets.ICapturedTimeRepository" />
    public sealed class CapturedTimeRepository : GenericRepository<CapturedTime>, ICapturedTimeRepository
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CapturedTimeRepository"/> class.
        /// </summary>
        public CapturedTimeRepository()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CapturedTimeRepository"/> class.
        /// </summary>
        /// <param name="dataContext">The data context.</param>
        public CapturedTimeRepository(DataContext dataContext)
            : base(dataContext)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets all dates.
        /// </summary>
        /// <param name="from">From date.</param>
        /// <param name="to">To date.</param>
        /// <returns>
        /// The date sates.
        /// </returns>
        public IEnumerable<DateState> GetAllDateStates(DateTime from, DateTime to)
        {
            EnsureNotDisposed();

            from = from.Date;
            to = to.Date;

            if (from > to)
                return null;

            IQueryable<DateState> query = DataContext
                .Set<DateState>()
                .Where(x => x.Date >= from && x.Date <= to);

            return query.AsEnumerable();
        }

        #endregion
    }
}
