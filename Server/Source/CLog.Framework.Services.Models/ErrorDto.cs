using CLog.Framework.Models;
using System.Globalization;
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
        /// Initializes a new instance of the <see cref="ErrorDto" /> class.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="code">The code.</param>
        /// <param name="message">The message.</param>
        /// <param name="additionalInfo">The additional information.</param>
        public ErrorDto(ErrorCategory category, string code, string message, string additionalInfo)
        {
            Category = category;
            Code = code;
            Message = message;
            AdditionalInfo = additionalInfo;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        [DataMember]
        public ErrorCategory Category { get; set; }

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

        #region Methods

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            string message = string.Format(CultureInfo.CurrentCulture, "({0}) {1}", Code, Message);

            if (!string.IsNullOrWhiteSpace(AdditionalInfo))
                message = string.Format(CultureInfo.CurrentCulture, "{0} {1}", AdditionalInfo);

            return message;
        }

        #endregion
    }
}
