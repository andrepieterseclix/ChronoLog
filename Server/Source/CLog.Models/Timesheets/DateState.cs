using CLog.Framework.Models;
using System;

namespace CLog.Models.Timesheets
{
    /// <summary>
    /// Represents the Date State business domain model.
    /// </summary>
    public class DateState : BusinessModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DateState"/> class.
        /// </summary>
        public DateState()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateState"/> class.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="isLocked">if set to <c>true</c> [is locked].</param>
        /// <param name="isPublicHoliday">if set to <c>true</c> [is public holiday].</param>
        public DateState(DateTime date, bool isLocked, bool isPublicHoliday)
        {
            Date = date;
            IsLocked = isLocked;
            IsPublicHoliday = isPublicHoliday;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this day is locked.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this day is locked; otherwise, <c>false</c>.
        /// </value>
        public bool IsLocked { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this day is public holiday.
        /// </summary>
        /// <value>
        /// <c>true</c> if this day is public holiday; otherwise, <c>false</c>.
        /// </value>
        public bool IsPublicHoliday { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new instance of the <see cref="DateState"/> class.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="isLocked">if set to <c>true</c> [is locked].</param>
        /// <param name="isPublicHoliday">if set to <c>true</c> [is public holiday].</param>
        /// <returns>A new date state.</returns>
        public static DateState New(DateTime date, bool isLocked, bool isPublicHoliday)
        {
            return new DateState(date, isLocked, isPublicHoliday);
        }

        #endregion
    }
}
