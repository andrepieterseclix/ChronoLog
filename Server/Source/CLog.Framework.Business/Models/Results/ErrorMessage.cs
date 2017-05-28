using CLog.Framework.Models;
using System;
using System.Globalization;

namespace CLog.Framework.Business.Models.Results
{
    /// <summary>
    /// Represents an error message model.
    /// </summary>
    public class ErrorMessage
    {
        private string v1;
        private string v2;
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorMessage" /> class.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="code">The code.</param>
        /// <param name="message">The message.</param>
        public ErrorMessage(ErrorCategory category, string code, string message)
        {
            Category = category;
            Code = code;
            Message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorMessage" /> class.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="code">The code.</param>
        /// <param name="message">The message.</param>
        /// <param name="ex">The ex.</param>
        public ErrorMessage(ErrorCategory category, string code, string message, Exception ex)
            : this(category, code, message)
        {
            Exception = ex;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorMessage" /> class.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="code">The code.</param>
        /// <param name="message">The message.</param>
        /// <param name="additionalInfo">The additional information.</param>
        public ErrorMessage(ErrorCategory category, string code, string message, string additionalInfo)
            : this(category, code, message)
        {
            AdditionalInfo = additionalInfo;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorMessage" /> class.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="code">The code.</param>
        /// <param name="message">The message.</param>
        /// <param name="additionalInfo">The additional information.</param>
        /// <param name="ex">The ex.</param>
        public ErrorMessage(ErrorCategory category, string code, string message, string additionalInfo, Exception ex)
            : this(category, code, message, additionalInfo)
        {
            Exception = ex;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public ErrorCategory Category { get; set; }

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
            return string.Format(CultureInfo.CurrentCulture, "({0}) {1}", Code, Message);
        }

        #endregion
    }
}
