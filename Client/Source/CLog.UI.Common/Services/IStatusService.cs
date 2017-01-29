namespace CLog.UI.Common.Services
{
    /// <summary>
    /// Represents the status service contract for setting the status bar text.
    /// </summary>
    public interface IStatusService
    {
        /// <summary>
        /// Sets the status.
        /// </summary>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="format">The format of the message.</param>
        /// <param name="args">The arguments.</param>
        void SetStatus(StatusMessageType messageType, string format, params object[] args);
    }
}
