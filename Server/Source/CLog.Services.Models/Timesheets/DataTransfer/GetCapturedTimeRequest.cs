using CLog.Framework.Services.Models;
using System;
using System.Runtime.Serialization;

namespace CLog.Services.Models.Timesheets.DataTransfer
{
    /// <summary>
    /// Represents the captured time request model.
    /// </summary>
    /// <seealso cref="CLog.Framework.Services.Models.RequestBase" />
    [DataContract]
    public sealed class GetCapturedTimeRequest : RequestBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GetCapturedTimeRequest"/> class.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        public GetCapturedTimeRequest(DateTime fromDate, DateTime toDate)
        {
            FromDate = fromDate;
            ToDate = toDate;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets from date.
        /// </summary>
        /// <value>
        /// From date.
        /// </value>
        [DataMember(IsRequired = true)]
        public DateTime FromDate { get; set; }

        /// <summary>
        /// Gets or sets to date.
        /// </summary>
        /// <value>
        /// To date.
        /// </value>
        [DataMember(IsRequired = true)]
        public DateTime ToDate { get; set; }

        #endregion
    }
}
