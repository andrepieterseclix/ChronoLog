using CLog.UI.Common.Helpers;
using CLog.UI.Common.Messaging;
using CLog.UI.Common.Messaging.Mediator;
using CLog.UI.Common.Messaging.Models;
using CLog.UI.Common.Services;
using System.Globalization;

namespace CLog.UI.Main.Services
{
    /// <summary>
    /// Represents the status service implementation for setting the status bar text.
    /// </summary>
    /// <seealso cref="CLog.UI.Common.Services.IStatusService" />
    public sealed class StatusService : IStatusService
    {
        /// <summary>
        /// Sets the status.
        /// </summary>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="format">The format of the message.</param>
        /// <param name="args">The arguments.</param>
        public void SetStatus(StatusMessageType messageType, string format, params object[] args)
        {
            string message = (args?.Length ?? 0) > 0
                ? string.Format(CultureInfo.CurrentCulture, format, args)
                : format;

            DispatcherHelper.Invoke(() =>
            {
                Mediator.Instance.NotifyColleaguesAsync(MessagingConstants.STATUS_UPDATE, new StatusMessage(messageType, message));
            });
        }
    }
}
