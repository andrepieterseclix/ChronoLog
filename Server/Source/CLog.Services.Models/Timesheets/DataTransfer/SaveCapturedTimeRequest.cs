using CLog.Framework.Services.Models;
using System.Runtime.Serialization;

namespace CLog.Services.Models.Timesheets.DataTransfer
{
    /// <summary>
    /// Represents the save captured time request model.
    /// </summary>
    /// <seealso cref="CLog.Framework.Services.Models.RequestBase" />
    [DataContract]
    public sealed class SaveCapturedTimeRequest : RequestBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveCapturedTimeRequest"/> class.
        /// </summary>
        public SaveCapturedTimeRequest()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveCapturedTimeRequest" /> class.
        /// </summary>
        /// <param name="items">The items.</param>
        public SaveCapturedTimeRequest(CapturedTimeDetailDto[] items)
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
        public CapturedTimeDetailDto[] CapturedTimeItems { get; set; }

        #endregion
    }
}
