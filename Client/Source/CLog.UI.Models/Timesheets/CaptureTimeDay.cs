using System;

namespace CLog.UI.Models.Timesheets
{
    /// <summary>
    /// Represents the Capture Time day model.
    /// </summary>
    /// <seealso cref="CLog.UI.Models.ModelBase" />
    public sealed class CaptureTimeDay : ModelBase
    {
        #region Fields

        private DateTime _date;

        private byte _hours;

        private bool _isLocked;

        private bool _isEnabled;

        private bool _isPublicHoliday;

        private bool _isToday;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CaptureTimeDay"/> class.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="hours">The hours.</param>
        /// <param name="isLocked">if set to <c>true</c> [is locked].</param>
        /// <param name="isEnabled">if set to <c>true</c> [is enabled].</param>
        /// <param name="isPublicHoliday">if set to <c>true</c> [is public holiday].</param>
        public CaptureTimeDay(DateTime date, byte hours, bool isLocked, bool isEnabled, bool isPublicHoliday)
        {
            Date = date;
            Hours = hours;
            IsLocked = isLocked;
            IsEnabled = isEnabled;
            IsPublicHoliday = isPublicHoliday;
            IsToday = date.Date == DateTime.Now.Date;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public DateTime Date
        {
            get { return _date; }
            set { SetProperty(ref _date, value); }
        }

        /// <summary>
        /// Gets or sets the hours.
        /// </summary>
        /// <value>
        /// The hours.
        /// </value>
        public byte Hours
        {
            get { return _hours; }
            set { SetProperty(ref _hours, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is locked.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is locked; otherwise, <c>false</c>.
        /// </value>
        public bool IsLocked
        {
            get { return _isLocked; }
            set { SetProperty(ref _isLocked, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { SetProperty(ref _isEnabled, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is public holiday.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is public holiday; otherwise, <c>false</c>.
        /// </value>
        public bool IsPublicHoliday
        {
            get { return _isPublicHoliday; }
            set { SetProperty(ref _isPublicHoliday, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is today.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is today; otherwise, <c>false</c>.
        /// </value>
        public bool IsToday
        {
            get { return _isToday; }
            set { SetProperty(ref _isToday, value); }
        }

        #endregion
    }
}
