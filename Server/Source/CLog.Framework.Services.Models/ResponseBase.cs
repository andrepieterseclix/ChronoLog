using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CLog.Framework.Services.Models
{
    /// <summary>
    /// Represents the Response base class.
    /// </summary>
    [DataContract]
    public abstract class ResponseBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseBase"/> class.
        /// </summary>
        public ResponseBase()
        {
            Errors = new List<ErrorDto>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the session has expired.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the session has session expired; otherwise, <c>false</c>.
        /// </value>
        [DataMember(IsRequired = true)]
        public bool SessionExpired { get; set; }

        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        [DataMember(IsRequired = true)]
        public List<ErrorDto> Errors { get; set; }

        #endregion
    }
}
