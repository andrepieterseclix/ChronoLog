using System;
using System.Globalization;

namespace CLog.UI.Common.Business
{
    /// <summary>
    /// Represents an error message model.
    /// </summary>
    public class ErrorMessage
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorMessage"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="message">The message.</param>
        public ErrorMessage(string code, string message)
        {
            Code = code;
            Message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorMessage"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="message">The message.</param>
        /// <param name="ex">The ex.</param>
        public ErrorMessage(string code, string message, Exception ex)
            : this(code, message)
        {
            Exception = ex;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorMessage"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="message">The message.</param>
        /// <param name="additionalInfo">The additional information.</param>
        public ErrorMessage(string code, string message, string additionalInfo)
            : this(code, message)
        {
            AdditionalInfo = additionalInfo;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorMessage"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="message">The message.</param>
        /// <param name="additionalInfo">The additional information.</param>
        /// <param name="ex">The ex.</param>
        public ErrorMessage(string code, string message, string additionalInfo, Exception ex)
            : this(code, message, additionalInfo)
        {
            Exception = ex;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public string Code { get; private set; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; private set; }

        /// <summary>
        /// Gets the additional information.
        /// </summary>
        /// <value>
        /// The additional information.
        /// </value>
        public string AdditionalInfo { get; set; }

        /// <summary>
        /// Gets the exception.
        /// </summary>
        /// <value>
        /// The exception.
        /// </value>
        public Exception Exception { get; set; }

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
                message = string.Format(CultureInfo.CurrentCulture, "{0} {1}", message, AdditionalInfo);

            return message;
        }

        #endregion
    }
}
