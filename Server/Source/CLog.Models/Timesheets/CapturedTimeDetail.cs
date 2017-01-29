using System;

namespace CLog.Models.Timesheets
{
    /// <summary>
    /// Represents the Captured Time Details transient model.
    /// </summary>
    public class CapturedTimeDetail
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CapturedTimeDetail"/> class.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="date">The date.</param>
        /// <param name="hoursWorked">The hours worked.</param>
        public CapturedTimeDetail(string userName, DateTime date, byte hoursWorked)
        {
            UserName = userName;
            Date = date.Date;
            HoursWorked = hoursWorked;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; set; }

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

        #endregion
    }
}
