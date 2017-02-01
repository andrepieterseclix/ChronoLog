using System;
using System.Runtime.Serialization;

namespace CLog.Services.Models.Timesheets
{
    /// <summary>
    /// Represents the Captured Time Detail data transfer object.
    /// </summary>
    /// <seealso cref="CLog.Services.Models.DtoBase" />
    [DataContract]
    public sealed class CapturedTimeDetailDto : DtoBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CapturedTimeDetailDto" /> class.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="date">The date.</param>
        /// <param name="hoursWorked">The hours worked.</param>
        public CapturedTimeDetailDto(string userName, DateTime date, byte hoursWorked)
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

        #endregion
    }
}
