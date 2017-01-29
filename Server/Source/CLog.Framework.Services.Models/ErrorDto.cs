using System.Runtime.Serialization;

namespace CLog.Framework.Services.Models
{
    /// <summary>
    /// Represents the Error data transfer object.
    /// </summary>
    [DataContract]
    public class ErrorDto
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorDto"/> class.
        /// </summary>
        public ErrorDto()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorDto"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="message">The message.</param>
        /// <param name="additionalInfo">The additional information.</param>
        public ErrorDto(string code, string message, string additionalInfo)
        {
            Code = code;
            Message = message;
            AdditionalInfo = additionalInfo;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [DataMember]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the additional information.
        /// </summary>
        /// <value>
        /// The additional information.
        /// </value>
        [DataMember]
        public string AdditionalInfo { get; set; }

        #endregion
    }
}
