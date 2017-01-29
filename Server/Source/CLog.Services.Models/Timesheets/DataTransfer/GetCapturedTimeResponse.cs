using CLog.Framework.Services.Models;
using System.Runtime.Serialization;

namespace CLog.Services.Models.Timesheets.DataTransfer
{
    /// <summary>
    /// Represents the captured time response model.
    /// </summary>
    /// <seealso cref="CLog.Framework.Services.Models.ResponseBase" />
    [DataContract]
    public sealed class GetCapturedTimeResponse : ResponseBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GetCapturedTimeResponse"/> class.
        /// </summary>
        public GetCapturedTimeResponse()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetCapturedTimeResponse"/> class.
        /// </summary>
        /// <param name="items">The items.</param>
        public GetCapturedTimeResponse(CapturedTimeDto[] items)
        {
            CapturedTimeItems = items;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the captured time items.
        /// </summary>
        /// <value>
        /// The captured time items.
        /// </value>
        [DataMember(IsRequired = true)]
        public CapturedTimeDto[] CapturedTimeItems { get; set; }

        #endregion
    }
}
