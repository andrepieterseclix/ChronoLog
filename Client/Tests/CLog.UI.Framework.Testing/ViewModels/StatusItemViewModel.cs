using CLog.UI.Common.Services;
using CLog.UI.Common.ViewModels;
using System;

namespace CLog.UI.Framework.Testing.ViewModels
{
    public class StatusItemViewModel : BasicViewModelBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusItemViewModel"/> class.
        /// </summary>
        /// <param name="statusType">Type of the status.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="message">The message.</param>
        public StatusItemViewModel(StatusMessageType statusType, DateTime timestamp, string message)
        {
            StatusType = statusType;
            Timestamp = timestamp;
            Message = message;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type of the status.
        /// </summary>
        /// <value>
        /// The type of the status.
        /// </value>
        public StatusMessageType StatusType { get; private set; }

        /// <summary>
        /// Gets the timestamp.
        /// </summary>
        /// <value>
        /// The timestamp.
        /// </value>
        public DateTime Timestamp { get; private set; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; private set; }

        #endregion
    }
}
