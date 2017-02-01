using System;
using System.Runtime.Serialization;

namespace CLog.Services.Models.Timesheets
{
    /// <summary>
    /// Represents the captured time data transfer object.
    /// </summary>
    [DataContract]
    public sealed class CapturedTimeDto : DtoBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CapturedTimeDto"/> class.
        /// </summary>
        public CapturedTimeDto()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CapturedTimeDto" /> class.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="date">The date.</param>
        /// <param name="hoursWorked">The hours worked.</param>
        /// <param name="isLocked">if set to <c>true</c> [is locked].</param>
        /// <param name="isPublicHoliday">if set to <c>true</c> [is public holiday].</param>
        /// <param name="isEnabled">if set to <c>true</c> [is enabled].</param>
        public CapturedTimeDto(string userName, DateTime date, byte hoursWorked, bool isLocked, bool isPublicHoliday, bool isEnabled)
        {
            UserName = userName;
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
        [DataMember(IsRequired = true)]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        [DataMember(IsRequired = true)]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the hours worked.
        /// </summary>
        /// <value>
        /// The hours worked.
        /// </value>
        [DataMember(IsRequired = true)]
        public byte HoursWorked { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this day is locked.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this day is locked; otherwise, <c>false</c>.
        /// </value>
        [DataMember(IsRequired = true)]
        public bool IsLocked { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this day is public holiday.
        /// </summary>
        /// <value>
        /// <c>true</c> if this day is public holiday; otherwise, <c>false</c>.
        /// </value>
        [DataMember(IsRequired = true)]
        public bool IsPublicHoliday { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this day is enabled for capturing time.
        /// </summary>
        /// <value>
        /// <c>true</c> if this day is enabled; otherwise, <c>false</c>.
        /// </value>
        [DataMember(IsRequired = true)]
        public bool IsEnabled { get; set; }

        #endregion
    }
}
