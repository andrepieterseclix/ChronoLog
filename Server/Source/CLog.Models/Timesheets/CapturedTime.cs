using CLog.Framework.Models;
using CLog.Models.Access;
using System;

namespace CLog.Models.Timesheets
{
    /// <summary>
    /// Represents the Captured Time business domain model.
    /// </summary>
    /// <seealso cref="CLog.Framework.Models.BusinessModel" />
    public class CapturedTime : BusinessModel
    {
        #region Fields

        private User _user;

        #endregion

        #region Constructors

        public CapturedTime()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CapturedTime" /> class.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="date">The date.</param>
        /// <param name="hoursWorked">The hours worked.</param>
        /// <param name="isLocked">if set to <c>true</c> [is locked].</param>
        /// <param name="isPublicHoliday">if set to <c>true</c> [is public holiday].</param>
        /// <param name="isEnabled">if set to <c>true</c> [is enabled].</param>
        public CapturedTime(long userId, DateTime date, byte hoursWorked, bool isLocked, bool isPublicHoliday, bool isEnabled)
        {
            UserId = userId;
            Date = date;
            HoursWorked = hoursWorked;
            IsLocked = isLocked;
            IsPublicHoliday = isPublicHoliday;
            IsEnabled = isEnabled;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public long UserId { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the hours worked.
        /// </summary>
        /// <value>
        /// The hours worked.
        /// </value>
        public byte HoursWorked { get; set; }

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

        /// <summary>
        /// Gets or sets a value indicating whether this day is enabled for capturing time.
        /// </summary>
        /// <value>
        /// <c>true</c> if this day is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public virtual User User
        {
            get { return _user; }
            set { _user = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new instance of the <see cref="CapturedTime" /> class.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="date">The date.</param>
        /// <param name="hoursWorked">The hours worked.</param>
        /// <returns>A new captured time instance.</returns>
        public static CapturedTime New(User user, DateTime date, byte hoursWorked)
        {
            CapturedTime model = new CapturedTime(0, date, hoursWorked, false, false, true);

            if (user != null)
            {
                model.UserId = user.Id;
                model.User = user;
            }

            return model;
        }

        #endregion
    }
}
