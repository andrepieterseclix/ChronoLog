using System;

namespace CLog.Common.Logging
{
    /// <summary>
    /// The logger contract.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Info(string message);

        /// <summary>
        /// Logs the specified error message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Error(string message);

        /// <summary>
        /// Logs the specified exception.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ex">The exception.</param>
        void Exception(string message, Exception ex);

        /// <summary>
        /// Logs the specified Fatal message and exception.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ex">The exception, can be <code>null</code>.</param>
        void Fatal(string message, Exception ex);

        /// <summary>
        /// Logs the specified debug message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Debug(string message);

        /// <summary>
        /// Logs the specified warning message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Warning(string message);
    }
}
