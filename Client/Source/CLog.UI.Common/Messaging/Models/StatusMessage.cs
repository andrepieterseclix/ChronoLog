using CLog.UI.Common.Services;

namespace CLog.UI.Common.Messaging.Models
{
    /// <summary>
    /// Represents a status message.
    /// </summary>
    public class StatusMessage
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusMessage"/> class.
        /// </summary>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="message">The message.</param>
        public StatusMessage(StatusMessageType messageType, string message)
        {
            Message = message;
            MessageType = messageType;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the type of the message.
        /// </summary>
        /// <value>
        /// The type of the message.
        /// </value>
        public StatusMessageType MessageType { get; set; }

        #endregion
    }
}
